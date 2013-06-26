using System;
using System.Linq;
using CiberIs.Models;

namespace CiberIs.Badges
{
    public class MorningGloryBadge : BadgeJob
    {
        private readonly IMongoDb _mongoDb;
        private const string BadgeName = "Morning glory - is før kl 9";

        public MorningGloryBadge(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        protected override void AwardBadges()
        {
            var badges = _mongoDb.GetCollection<Badge>(CollectionName).ToList();
            var purchases = _mongoDb.GetCollection<Purchase>("Purchases").ToList().Where(x => x.Time != null && x.Time.Value.Hour < 9).GroupBy(x => x.Buyer).Select(x => x.First().Buyer).ToList();
            BadgeRepository.AddBadges(purchases, badges, _mongoDb, BadgeName);
        }

        protected override TimeSpan Interval
        {
            get { return DefaultTimeSpan; }
        }
    }
}