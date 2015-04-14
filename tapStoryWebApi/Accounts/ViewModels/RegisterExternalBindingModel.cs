using System.ComponentModel.DataAnnotations;

namespace tapStoryWebApi.Accounts.ViewModels
{
    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}