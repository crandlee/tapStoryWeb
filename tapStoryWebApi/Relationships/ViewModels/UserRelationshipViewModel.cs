using System.ComponentModel.DataAnnotations;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Relationships.ViewModels
{
    public class UserRelationshipViewModel
    {
        [Key]
        public int Id { get; set; }

        public RelationshipType RelationshipType { get; set; }

        public ApplicationUser RelationshipWith { get; set; }
    }
}