using System.ComponentModel.DataAnnotations;

namespace tapStoryWebApi.Accounts.ViewModels
{
    public class AddRoleBindingModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string RoleName { get; set; }

    }
}