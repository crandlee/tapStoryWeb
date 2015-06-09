using System.ComponentModel.DataAnnotations;

namespace tapStoryWebApi.Accounts.DTO
{
    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}