using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EmptyMvc4.Models;
using WebMatrix.WebData;

namespace CiberIs
{
    public class DbInitializer : DropCreateDatabaseAlways<UsersContext>
    {
        protected override void Seed(UsersContext context)
        {
            SeedMembership();
        }

        private void SeedMembership()
        {
            WebSecurity.InitializeDatabaseConnection("UsersContext",
                "UserProfiles", "UserId", "UserName", autoCreateTables: true);
        }
    }
}