﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using tapStoryWebApi.Common.ActionResults;
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Common.Helpers;
using tapStoryWebApi.Relationships.Security;
using tapStoryWebApi.Relationships.Services;
using tapStoryWebApi.Relationships.ViewModels;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Relationships.APIControllers
{
    public class FriendshipsController: ApiController
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
        //Retrieve UserRelationships for any friendship for the user Id in the URI
        //URI PARAMS: {sourceUserId} ?status={active, pending, invalid}
        [AcceptVerbs("GET")]
        [Route("api/Friendships/{sourceUserId}")]
        public async Task<IHttpActionResult> Get([FromUri] int sourceUserId)
        {
            var statusFilter = ControllerHelper.GetQueryStringValue(ActionContext, "status");
            var friendRelationships =
                _userRelSecurity.SecureFriendQuery(_userRelService.GetFriendRelationships(sourceUserId));
            if (!String.IsNullOrEmpty(statusFilter))
            {
                friendRelationships = FilterOnRelationshipStatus(friendRelationships, statusFilter);
            }
            return Ok(await friendRelationships.ToListAsync());
        }


        //POST
        //Create a new pending ack relationship
        //BODY PARAMS: {sourceUserId, targetUserId}
        [AcceptVerbs("POST")]
        [Route("api/Friendships")]
        public async Task<IHttpActionResult> CreateFriendRequest([FromBody] SourceTargetUser stu)
        {
            if (!_userRelSecurity.CanCreatePendingFriendship()) return Unauthorized();
            var newPendingFriendship = await _userRelService.CreatePendingFriendship(stu.SourceUserId, stu.TargetUserId);

            if (newPendingFriendship == null) return Ok();
            await _ctx.SaveChangesAsync();
            return new CreatedActionResult<FriendRelationshipViewModel>(Request, newPendingFriendship);    
        }

        public class SourceTargetUser
        {
            public int SourceUserId { get; set; }
            public int TargetUserId { get; set; }
        }

        //PUT
        //Acknowledge pending ack relationship
        //BODY PARAMS: {sourceUserId, targetUserId}

        //PATCH
        //NONE

        //DELETE
        //Set to INACTIVE any existing friendship of any current status
        //BODY PARAMS: {sourceUserId, targetUserId}


        private static IQueryable<FriendRelationshipViewModel> FilterOnRelationshipStatus(IQueryable<FriendRelationshipViewModel> friendRelationships, string statusQueryString)
        {           
            switch (statusQueryString)
            {
                case "active":
                    return friendRelationships.Where(fr => fr.RelationshipStatus == RelationshipStatus.Active);
                case "pending":
                    return friendRelationships.Where(fr =>  new[]{RelationshipStatus.Pending,RelationshipStatus.PendingAck }.Contains(fr.RelationshipStatus));
                case "inactive":
                    return friendRelationships.Where(fr => fr.RelationshipStatus == RelationshipStatus.Inactive);
                default:
                    return friendRelationships;
            }    
        }

    }
}