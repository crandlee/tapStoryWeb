using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tapStoryWebApi.Relationships.ViewModels;

namespace tapStoryWebApi.Accounts.ViewModels
{
    public class ApplicationUserViewModel
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string UserName { get; set; }

        public IEnumerable<UserRelationshipViewModel> UserRelationships { get; set; } 

    }
}