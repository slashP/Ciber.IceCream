namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using System.Linq;
    using CiberIs.Models;

    public class MorningGloryBadge : IGrantBadges
    {
        public const string BadgeName = "Morning glory - is før kl 9";

        public bool HasBadge(IEnumerable<Purchase> purchases)
        {
            var hasBadge = purchases.Any(x => x.Time != null && x.Time.Value.Hour < 9);
            return hasBadge;
        }
    }
}