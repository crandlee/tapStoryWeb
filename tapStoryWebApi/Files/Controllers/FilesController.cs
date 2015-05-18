using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.OData;
using System.Web.OData.Query;
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Exceptions;
using tapStoryWebApi.Files.Services;
using tapStoryWebData.EF.Contexts;

namespace tapStoryWebApi.Files.Controllers
{
    [Authorize]
    public class FilesController : ODataController
    {
        private ApplicationDbContext _ctx;
        private FileService _fileService;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            _ctx = ServiceFactory.GetDbContext<ApplicationDbContext>(controllerContext.Request);
            _fileService = ServiceFactory.GetFileService(_ctx, controllerContext.Request, controllerContext.RequestContext.Principal);
            base.Initialize(controllerContext);
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            try
            {
                return Ok(_fileService.GetFiles(key));
            }
            catch (Exception e)
            {
                return new InternalErrorActionResult(this, e, "Get");
            }
        }

    }
}