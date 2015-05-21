using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.OData;
using System.Web.OData.Query;
using tapStoryWebApi.Attributes;
using tapStoryWebApi.Common.ActionResults;
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Common.Helpers;
using tapStoryWebApi.Files.Services;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Files.Controllers
{
    public class FileGroupsController : ODataController
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
        public IHttpActionResult Get([FromODataUri] int key)
        {
            try
            {
                return Ok(_fileDataService.GetFileGroup(key));
            }
            catch (Exception e)
            {
                return new InternalErrorActionResult(this, e);                
            }
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult GetFiles([FromODataUri] int key)
        {
            try
            {
                return Ok(_fileDataService.GetFiles(key));
            }
            catch (Exception e)
            {
                return new InternalErrorActionResult(this, e);
            }
        }

        //public async void DeleteRef([FromODataUri] int key, string navigationProperty,
        //    [FromBody] Uri link)
        //{
        //    ReferenceFunctionHelper.CallReferenceFunction("FileGroups", navigationProperty, ReferenceServiceFunctionType.Delete, _fileDataService, key, Request, link);
        //    await _ctx.SaveChangesAsync();
        //}

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}