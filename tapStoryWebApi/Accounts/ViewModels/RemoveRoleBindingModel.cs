using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace tapStoryWebApi.Accounts.ViewModels
{
    public class RemoveRoleBindingModel
    {
        [Required]
        [Integer]
        public int UserId { get; set; }

        [Required]
        public string RoleName { get; set; }

    }
}