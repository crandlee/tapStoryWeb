using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Relationships.ViewModels
{
    public class ChildRelationshipViewModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public RelationshipStatus RelationshipStatus { get; set; }
        public ApplicationUser Parent { get; set; }
        public ApplicationUser Child { get; set; }
    }
}