using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace tapStoryWebData.EF.Models
{
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {

        [StringLength(256), Required]
        public string FirstName { get; set; }
        
        [StringLength(256), Required]
        public string LastName { get; set; }

        [ForeignKey("SecondaryMemberId")]
        public ICollection<UserRelationship> SecondaryRelationships { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsMinor { get; set; }

        [ForeignKey("PrimaryMemberId")]
        public ICollection<UserRelationship> PrimaryRelationships { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

}
