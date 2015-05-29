using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.OData;
using System.Web.OData.Query;
using tapStoryWebApi.Common.ActionResults;
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Files.Services;
using tapStoryWebData.EF.Contexts;

namespace tapStoryWebApi.Files.ODataControllers
{
    public class OdUserStoriesController: ODataController
    {
        private ApplicationDbContext _ctx;
        private FileDataService _fileDataService;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            _ctx = ServiceFactory.GetDbContext<ApplicationDbContext>(controllerContext.Request);
            _fileDataService = ServiceFactory.GetFileService(_ctx, controllerContext.Request, controllerContext.RequestContext.Principal);
            base.Initialize(controllerContext);
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult Get()
        {
            return Ok(_fileDataService.GetUserFileGroups());
        }

        public IHttpActionResult Get([FromODataUri] int key)
        {
            return Ok(_fileDataService.GetUserFileGroups(key));
        }

        public IHttpActionResult GetOdFiles([FromODataUri] int key)
        {
            return Ok(_fileDataService.GetFileGroupFiles(key));            
        } 

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }

    }
}