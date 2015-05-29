using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.OData;
using System.Web.OData.Query;
using tapStoryWebApi.Common.ActionResults;
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Relationships.Security;
using tapStoryWebApi.Relationships.Services;
using tapStoryWebData.EF.Contexts;

namespace tapStoryWebApi.Relationships.ODataControllers
{
    public class OdGuardiansController: ODataController
    {

        private ApplicationDbContext _ctx;
        private UserRelationshipService _userRelService;
        private UserRelationshipSecurity _userRelSecurity;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            _ctx = ServiceFactory.GetDbContext<ApplicationDbContext>(controllerContext.Request);
            _userRelService = ServiceFactory.GetUserRelationshipService(_ctx, controllerContext.Request,
                controllerContext.RequestContext.Principal);
            _userRelSecurity = SecurityFactory.GetUserRelationshipSecurity(controllerContext.RequestContext.Principal);
            base.Initialize(controllerContext);
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_userRelSecurity.SecureGuardianQuery(_userRelService.GetGuardianRelationships()));
            }
            catch (Exception e)
            {
                return new InternalErrorActionResult(this, e);
            }
        }


        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}