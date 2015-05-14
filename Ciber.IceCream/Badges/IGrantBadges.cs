namespace CiberIs.Badges
{
    using System.Collections.Generic;
    using CiberIs.Models;

    public interface IGrantBadges
    {
        string BadgeName { get; }
        bool HasBadge(IEnumerable<Purchase> purchases);
    }
}