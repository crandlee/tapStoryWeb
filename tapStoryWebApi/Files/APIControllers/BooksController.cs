using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using tapStoryWebApi.Common.ActionResults;
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Files.Services;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Files.APIControllers
{
    public class BooksController : ApiController
    {
        private LocalPhysicalFileService _fileContentService;
        private ApplicationDbContext _ctx;
        private FileDataService _fileDataService;
        private FileGroupContentCommon _fileGroupContentCommon;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            _ctx = ServiceFactory.GetDbContext<ApplicationDbContext>(controllerContext.Request);
            _fileDataService = ServiceFactory.GetFileService(_ctx, controllerContext.Request, controllerContext.RequestContext.Principal);
            _fileContentService = new LocalPhysicalFileService(ConfigurationManager.AppSettings["BaseUploadsFolder"]);
            _fileGroupContentCommon = new FileGroupContentCommon(this, _ctx, _fileContentService, _fileDataService, false, FileGroupType.Book);
            base.Initialize(controllerContext);
        }

        //ADD FILE GROUP - RETURNS FILE GROUP SERVER REPRESENTATION
        [AcceptVerbs("POST")]
        [Route("api/Books")]
        public async Task<IHttpActionResult> CreateFileGroup()
        {
            if (!Request.Content.IsMimeMultipartContent()) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            return await _fileGroupContentCommon.SaveFileGroup(FileGroupContentCommon.FileGroupCreateMode.Create);
        }



        //UPDATE EXISTING FILE GROUP - OVERWRITE ALL
        [AcceptVerbs("PUT")]
        [Route("api/Books")]
        public async Task<IHttpActionResult> OverwriteFileGroup()
        {
            if (!Request.Content.IsMimeMultipartContent()) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            return await _fileGroupContentCommon.SaveFileGroup(FileGroupContentCommon.FileGroupCreateMode.Overwrite);

        }

        //UPDATE EXISTING FILE GROUP - ADD ONLY NEW
        [AcceptVerbs("PATCH")]
        [Route("api/Books")]
        public async Task<IHttpActionResult> UpdateFileGroup()
        {
            if (!Request.Content.IsMimeMultipartContent()) throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            return await _fileGroupContentCommon.SaveFileGroup(FileGroupContentCommon.FileGroupCreateMode.Edit);

        }

        //DELETE FILE GROUP
        [AcceptVerbs("DELETE")]
        [Route("api/Books/{fileGroupServerId}")]
        public async Task<IHttpActionResult> DeleteFileGroup(string fileGroupServerId)
        {

            return await _fileGroupContentCommon.DeleteFileGroup(fileGroupServerId);

        }

        //GET SERVER REPRESENTATION OF FILE GROUP
        [Route("api/Books/{fileGroupServerId}")]
        public IHttpActionResult Get(string fileGroupServerId)
        {
            return _fileGroupContentCommon.GetFileGroup(fileGroupServerId);
        }

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }

    }
}