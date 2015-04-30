namespace CiberIs.Badges
{
    using System.Collections.Generic;

    public interface IBadgeService
    {
        IEnumerable<string> UpdateBadgeForEmployee(int employeeId);
    }
}