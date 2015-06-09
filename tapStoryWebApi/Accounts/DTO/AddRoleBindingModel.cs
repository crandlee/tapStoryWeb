using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace tapStoryWebApi.Accounts.DTO
{
    public class AddRoleBindingModel
    {
        [Required]
        [Integer]
        public int UserId { get; set; }

        [Required]
        public string RoleName { get; set; }

    }
}