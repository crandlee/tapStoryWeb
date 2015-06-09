using tapStoryWebApi.Accounts.DTO;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Relationships.DTO
{
    public class FriendRelationshipModel
    {
        public int Id { get; set; }
        public int SourceFriendId { get; set; }
        public int TargetFriendId { get; set; }
        public RelationshipStatus RelationshipStatus { get; set; }
        public ApplicationUserViewModel SourceFriend { get; set; }
        public ApplicationUserViewModel TargetFriend { get; set; }
    }
}