using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Relationships.Security;
using tapStoryWebApi.Relationships.Services;
using tapStoryWebData.EF.Contexts;

namespace tapStoryWebApi.Relationships.APIControllers
{
    public class GuardianshipsController : ApiController
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
        //Retrieve Guardians for a child for the user Id in the URI
        //URI PARAMS: {sourceUserId} 
        [AcceptVerbs("GET")]
        [Route("api/Guardianships/{sourceUserId}")]
        public async Task<IHttpActionResult> Get([FromUri] int sourceUserId)
        {
            var guardianRelationships = _userRelSecurity.SecureGuardianQuery(_userRelService.GetGuardianRelationships(sourceUserId));
            return Ok(await guardianRelationships.ToListAsync());
        }

    }
}