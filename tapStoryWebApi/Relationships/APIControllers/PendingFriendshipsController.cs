using System.Web.Http;

namespace tapStoryWebApi.Relationships.APIControllers
{
    public class PendingFriendshipsController: ApiController
    {

        //GET
        //Retrieve list UserRelationships for friendships that have the status PendingAck
        //URI PARAMS: {sourceUserId}

        //POST
        //Acknowledge Pending Friendship making the UserRelationship status Active

        //PUT
        //NONE

        //PATCH
        //NONE

        //DELETE
        //Set to INACTIVE any existing friendship of any current status
        //BODY PARAMS: {sourceUserId, targetUserId}

    }
}