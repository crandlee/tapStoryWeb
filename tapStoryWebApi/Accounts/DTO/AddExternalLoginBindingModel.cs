using System.ComponentModel.DataAnnotations;

namespace tapStoryWebApi.Accounts.DTO
{
    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }
}