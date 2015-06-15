namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using System.Linq;
    using CiberIs.Models;

    public class SilverBadge : IGrantBadges
    {
        public string BadgeName { get { return "Silver"; } }

        public bool HasBadge(IEnumerable<Purchase> purchases)
        {
            return purchases.Count() >= 50;
        }
    }
}