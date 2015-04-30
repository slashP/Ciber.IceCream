namespace CiberIs.Badges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CiberIs.Models;

    public class OneFavouriteBadge : IGrantBadges
    {
        public const string BadgeName = "Trofast - samme is fem ganger på rad";

        public bool HasBadge(IEnumerable<Purchase> purchases)
        {
            var longest = 0;
            var temp = 1;
            var lastIteration = purchases.First();
            foreach (var purchase in
                purchases.OrderBy(x => x.Time)
                         .Where(x => !string.IsNullOrEmpty(x.IceCreamId))
                         .Skip(1))
            {
                var sameBuy = lastIteration.IceCreamId == purchase.IceCreamId
                                  ? 1
                                  : 0;
                temp = (temp + sameBuy) * sameBuy;
                longest = Math.Max(temp, longest);
                lastIteration = purchase;
            }

            return longest >= 5;
        }
    }
}