using System.Collections.Generic;
using System.Linq;
using CiberIs.Models;

namespace CiberIs.Badges
{
    public static class BadgeRepository
    {
        private static void AddFirstBadge(IMongoDb mongoDb, int buyer, string badgeName, string collectionName)
        {
            var badge = new Badge { Ansattnummer = buyer, BadgesForUser = new List<string>() };
            badge.BadgesForUser.Add(badgeName);
            mongoDb.Insert(badge, collectionName); // First badge awarded
        }

        public static void AddBadges(IEnumerable<int> buyerIds, List<Badge> badges, IMongoDb mongoDb, string badgeName)
        {
            foreach (var buyer in buyerIds)
            {
                var buyerBadge = badges.FirstOrDefault(x => x.Ansattnummer == buyer);
                if (buyerBadge != null && buyerBadge.BadgesForUser.Contains(badgeName)) continue; // Badge exists
                if (buyerBadge == null)
                {
                    AddFirstBadge(mongoDb, buyer, badgeName, BadgeJob.CollectionName);
                    continue;
                }
                buyerBadge.BadgesForUser.Add(badgeName);
                mongoDb.Save(buyerBadge, BadgeJob.CollectionName); // New badge
            }
        }
    }
}