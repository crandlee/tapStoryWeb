using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace tapStoryWebData.EF.Models
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, int, ApplicationUserRole>
    {
        public ApplicationRoleStore(DbContext context) : base(context)
        {
        }
    }
}
