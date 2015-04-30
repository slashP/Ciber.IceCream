namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using CiberIs.Models;

    internal interface IGrantBadges
    {
        bool HasBadge(IEnumerable<Purchase> purchases);
    }
}