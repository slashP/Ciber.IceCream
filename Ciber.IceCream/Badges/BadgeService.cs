namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using System.Linq;
    using CiberIs.Models;

    public class BadgeService : IBadgeService
    {
        const string CollectionName = "Badges";

        static readonly Dictionary<string, IGrantBadges> _grantBadges = new Dictionary
            <string, IGrantBadges>
        {
            { ConsistentBadge.BadgeName, new ConsistentBadge() }, 
            { GoldBadge.BadgeName, new GoldBadge() }, 
            { SilverBadge.BadgeName, new SilverBadge() }, 
            { BronzeBadge.BadgeName, new BronzeBadge() }, 
            { MixItUpBadge.BadgeName, new MixItUpBadge() }, 
            { MorningGloryBadge.BadgeName, new MorningGloryBadge() }, 
            { OneFavouriteBadge.BadgeName, new OneFavouriteBadge() }
        };

        readonly IMongoDb _mongoDb;

        public BadgeService(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        public IEnumerable<string> UpdateBadgeForEmployee(int employeeId)
        {
            var badge =
                _mongoDb.GetCollection<Badge>(CollectionName)
                        .FirstOrDefault(x => x.Ansattnummer == employeeId);
            if (badge == null)
            {
                badge = new Badge
                {
                    Ansattnummer = employeeId, 
                    BadgesForUser = new List<string>()
                };
                _mongoDb.Insert(badge, CollectionName);
            }

            var badgesForUser = badge.BadgesForUser.Select(x => x).ToList();
            var badgesGranted = new List<string>();
            var purchases =
                _mongoDb.GetCollection<Purchase>("Purchases")
                        .Where(x => x.Buyer == employeeId)
                        .ToList();
            foreach (var badgeToGrant in
                _grantBadges.Where(x => badgesForUser.Contains(x.Key) == false)
                            .Where(badgeToGrant => badgeToGrant.Value.HasBadge(purchases)))
            {
                badge.BadgesForUser.Add(badgeToGrant.Key);
                badgesGranted.Add(badgeToGrant.Key);
            }

            if (badgesGranted.Any())
            {
                _mongoDb.Save(badge, CollectionName);
            }

            return badgesGranted;
        }
    }
}