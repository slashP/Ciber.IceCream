namespace CiberIs.Badges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CiberIs.Models;

    public class BadgeService : IBadgeService
    {
        const string CollectionName = "Badges";

        public static readonly Dictionary<string, IGrantBadges> AvailableBadges =
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(p => typeof(IGrantBadges).IsAssignableFrom(p) && !p.IsInterface)
                    .Select(x => (IGrantBadges)Activator.CreateInstance(x))
                    .ToDictionary(x => x.BadgeName, x => x);

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
                AvailableBadges.Where(x => badgesForUser.Contains(x.Key) == false)
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