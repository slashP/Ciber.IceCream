namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using System.Linq;
    using CiberIs.Models;

    public class SilverBadge : IGrantBadges
    {
        public const string BadgeName = "Silver";

        public bool HasBadge(IEnumerable<Purchase> purchases)
        {
            return purchases.Count() >= 100;
        }
    }
}