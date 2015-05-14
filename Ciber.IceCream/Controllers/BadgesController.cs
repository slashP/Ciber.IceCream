namespace CiberIs.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using CiberIs.Badges;

    public class BadgesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return BadgeService.AvailableBadges.Keys;
        } 
    }
}