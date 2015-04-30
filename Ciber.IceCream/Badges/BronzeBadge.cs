namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using System.Linq;
    using CiberIs.Models;

    public class BronzeBadge : IGrantBadges
    {
        public const string BadgeName = "Bronze";

        public bool HasBadge(IEnumerable<Purchase> purchases)
        {
            return purchases.Count() >= 100;
        }
    }
}