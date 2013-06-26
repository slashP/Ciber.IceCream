using System;
using System.Web;
using System.Web.Caching;

namespace CiberIs.Badges
{
    public abstract class BadgeJob
    {
        private static readonly object SyncObject = new object();
        public const string CollectionName = "Badges";
        protected BadgeJob()
        {
            //start cycling on initialization
            Insert();
        }

        //override to provide specific badge logic
        protected abstract void AwardBadges();

        //how long to wait between iterations
        protected abstract TimeSpan Interval { get; }

        private void Callback(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason != CacheItemRemovedReason.Expired) return;
            lock (SyncObject)
            {
                AwardBadges();
            }
            Insert();
        }

        private void Insert()
        {
            HttpRuntime.Cache.Add(GetType().ToString(),
                this,
                null,
                Cache.NoAbsoluteExpiration,
                Interval,
                CacheItemPriority.Normal,
                Callback);
        }


        protected static TimeSpan DefaultTimeSpan
        {
            get { return new TimeSpan(0, 5, 0); }
        }
    }
}