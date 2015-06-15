namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using System.Linq;
    using CiberIs.Models;

    public class MixItUpBadge : IGrantBadges
    {
        public string BadgeName { get { return "Mix It Up - 10 forskjellige is"; } }

        public bool HasBadge(IEnumerable<Purchase> purchases)
        {
            var hasBadge =
                purchases.Where(x => !string.IsNullOrEmpty(x.IceCreamId))
                         .Select(x => x.IceCreamId)
                         .Distinct()
                         .Count() >= 10;
            return hasBadge;
        }
    }
}