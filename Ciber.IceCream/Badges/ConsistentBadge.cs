using System;
using System.Collections.Generic;
using System.Linq;
using CiberIs.Models;
using CiberIs.Extensions;

namespace CiberIs.Badges
{
    public class ConsistentBadge : BadgeJob
    {
        private const string BadgeName = "Is tre dager på rad";
        private readonly IMongoDb _mongoDb;

        public ConsistentBadge(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        protected override void AwardBadges()
        {
            var badges = _mongoDb.GetCollection<Badge>(CollectionName).ToList();
            var groupBy = _mongoDb.GetCollection<Purchase>("Purchases").ToList().GroupBy(x => x.Buyer);
            var purchaseDates = groupBy.Select(x => new { Dates = x.Where(y => y.Time != null).OrderBy(y => y.Time).Select(y => y.Time.Value.Date).Distinct(), x.First().Buyer }).ToList();
            var purchases = new List<int>();
            foreach (var t in purchaseDates)
            {
                var dateTimes = t.Dates.ToList();
                var allDates = new List<DateTime>();
                for (var date = dateTimes.First(); date <= dateTimes.Last(); date = date.AddDays(1))
                    allDates.Add(date);
                var buyArray = new bool[allDates.Count];
                allDates.Each((d, i) => buyArray[i] = dateTimes.Contains(d));
                var longest = 0;
                var temp = 0;
                foreach (var boughtThisDay in buyArray.Select(x => x ? 1 : 0))
                {
                    temp = (temp + boughtThisDay)*boughtThisDay;
                    longest = Math.Max(temp, longest);
                }
                if(longest >= 3)
                    purchases.Add(t.Buyer);
            }
            BadgeRepository.AddBadges(purchases, badges, _mongoDb, BadgeName);
        }

        protected override TimeSpan Interval
        {
            get { return DefaultTimeSpan; }
        }
    }
}