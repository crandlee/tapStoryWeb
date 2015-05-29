using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Relationships.ViewModels
{
    public class FriendRelationshipViewModel
    {
        public int Id { get; set; }
        public int SourceFriendId { get; set; }
        public int TargetFriendId { get; set; }
        public RelationshipStatus RelationshipStatus { get; set; }
        public ApplicationUser SourceFriend { get; set; }
        public ApplicationUser TargetFriend { get; set; }
    }
}