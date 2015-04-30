namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using System.Linq;
    using CiberIs.Models;

    public class GoldBadge : IGrantBadges
    {
        public const string BadgeName = "Gold";

        public bool HasBadge(IEnumerable<Purchase> purchases)
        {
            return purchases.Count() >= 100;
        }
    }
}