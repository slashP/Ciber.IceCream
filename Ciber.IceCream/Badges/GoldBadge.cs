namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using System.Linq;
    using CiberIs.Models;

    public class GoldBadge : IGrantBadges
    {
        public string BadgeName { get { return "Gold"; } }

        public bool HasBadge(IEnumerable<Purchase> purchases)
        {
            return purchases.Count() >= 100;
        }
    }
}