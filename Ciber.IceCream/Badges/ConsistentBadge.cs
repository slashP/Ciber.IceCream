using System;
using System.Collections.Generic;
using System.Linq;
using CiberIs.Models;
using CiberIs.Extensions;

namespace CiberIs.Badges
{
    public class ConsistentBadge : IGrantBadges
    {
        public const string BadgeName = "Is tre dager på rad";

        public bool HasBadge(IEnumerable<Purchase> purchases)
        {
            var purchaseDates = purchases.Where(y => y.Time != null).OrderBy(y => y.Time).Select(y => y.Time.Value.Date).Distinct().ToList();
                var allDates = new List<DateTime>();
            for (var date = purchaseDates.First();
                 date <= purchaseDates.Last();
                 date = date.AddDays(1))
            {
                allDates.Add(date);
            }

            var buyArray = new bool[allDates.Count];
            allDates.Each((d, i) => buyArray[i] = purchaseDates.Contains(d));
            var longest = 0;
            var temp = 0;
            foreach (var boughtThisDay in buyArray.Select(x => x ? 1 : 0))
            {
                temp = (temp + boughtThisDay) * boughtThisDay;
                longest = Math.Max(temp, longest);
            }

            return longest >= 3;
        }
    }
}