using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace tapStoryWebData.EF.Models
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUserStore(DbContext context) : base(context)
        {
        }
    }
}
