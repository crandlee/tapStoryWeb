using tapStoryWebApi.Accounts.ViewModels;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Relationships.ViewModels
{
    public class ChildRelationshipViewModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public RelationshipStatus RelationshipStatus { get; set; }
        public ApplicationUserViewModel Parent { get; set; }
        public ApplicationUserViewModel Child { get; set; }
    }
}