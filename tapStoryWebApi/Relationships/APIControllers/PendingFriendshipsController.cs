using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using tapStoryWebApi.Common.ActionResults;
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Common.Helpers;
using tapStoryWebApi.Relationships.DTO;
using tapStoryWebApi.Relationships.Security;
using tapStoryWebApi.Relationships.Services;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Relationships.APIControllers
{
    public class PendingFriendshipsController: ApiController
    {
        private ApplicationDbContext _ctx;
        private UserRelationshipService _userRelService;
        private UserRelationshipSecurity _userRelSecurity;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            _ctx = ServiceFactory.GetDbContext<ApplicationDbContext>(controllerContext.Request);
            _userRelService = ServiceFactory.GetUserRelationshipService(_ctx, controllerContext.Request,
                controllerContext.RequestContext.Principal);
            _userRelSecurity = SecurityFactory.GetUserRelationshipSecurity(_ctx, controllerContext.RequestContext.Principal);
            base.Initialize(controllerContext);
        }

        //GET
        //Retrieve list UserRelationships for friendships that have the status PendingAck
        //URI PARAMS: {sourceUserId}
        [AcceptVerbs("GET")]
        [Route("api/PendingFriendships/{sourceUserId}")]
        public async Task<IHttpActionResult> Get([FromUri] int sourceUserId)
        {
            var friendRelationships =
                _userRelSecurity.SecureFriendQuery(_userRelService.GetFriendRelationships(sourceUserId));
            friendRelationships =
                friendRelationships.Where(
                    fr =>
                        new[] {RelationshipStatus.Pending, RelationshipStatus.PendingAck}.Contains(fr.RelationshipStatus));                
            return Ok(await friendRelationships.ToListAsync());
        }

        //POST
        //NONE

        //PUT
        [AcceptVerbs("PUT")]
        [Route("api/PendingFriendships")]
        public async Task<IHttpActionResult> AcknowledgeFriendRequest([FromBody] FriendshipsController.SourceTargetUser stu)
        {
            if (!_userRelSecurity.CanAcceptPendingFriendship(stu.SourceUserId)) return Unauthorized();
            var newActiveFriendship = await _userRelService.CreatePendingFriendship(stu.SourceUserId, stu.TargetUserId);

            if (newActiveFriendship == null) return NotFound();
            await _ctx.SaveChangesAsync();
            return new CreatedActionResult<FriendRelationshipModel>(Request, newActiveFriendship);
        }

        //PATCH
        //NONE

        //DELETE
        //Set to INACTIVE any existing friendship of any current status
        //BODY PARAMS: {sourceUserId, targetUserId}
        [AcceptVerbs("DELETE")]
        [Route("api/PendingFriendships")]
        public async Task<IHttpActionResult> Unfriend([FromBody] FriendshipsController.SourceTargetUser stu)
        {
            if (!_userRelSecurity.CanUnfriend(stu.SourceUserId)) return Unauthorized();
            await _userRelService.Unfriend(stu.SourceUserId, stu.TargetUserId);

            await _ctx.SaveChangesAsync();
            return Ok();
        }

    }
}