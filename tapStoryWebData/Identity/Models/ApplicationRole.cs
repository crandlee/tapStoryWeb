using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace tapStoryWebData.Identity.Models
{
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>
    {
        public ApplicationRole()
        {
        }


        public ApplicationRole(string name, string description)
        {
            Name = name;
            Description = description;
        }


        [StringLength(512)]
        public string Description { get; set; }

    }
}
