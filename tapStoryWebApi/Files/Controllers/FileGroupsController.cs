using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.OData;
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Files.Services;
using tapStoryWebData.Identity.Contexts;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Files.Controllers
{
    [Authorize]
    public class FileGroupsController : ODataController
    {

        private ApplicationDbContext _ctx;
        private FileService _fileService;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            _ctx = ServiceFactory.GetDbContext<ApplicationDbContext>(controllerContext.Request);
            _fileService = ServiceFactory.GetFileService(_ctx, controllerContext.Request, controllerContext.RequestContext.Principal);
            base.Initialize(controllerContext);
        }

        public async Task<IHttpActionResult> Post(FileGroup fg)
        {
            
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _fileService.AddFileGroup(fg);
            await _ctx.SaveChangesAsync();
            return Created(fg);

        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<FileGroup> fg)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = await _ctx.FileGroups.FindAsync(key);
            if (entity == null) return NotFound();
            fg.Patch(entity);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_ctx.FileGroups.Any(p => p.Id == key))
                {
                    return NotFound();
                }
                throw;
            }
            return Updated(entity);
        }

        public async Task<IHttpActionResult> Put([FromODataUri] int key, FileGroup update)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (key != update.Id) return BadRequest();
            _ctx.Entry(update).State = EntityState.Modified;
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_ctx.FileGroups.Any(p => p.Id == key))
                {
                    return NotFound();
                }
                throw;                
            }
            return Updated(update);
        }


        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var entity = await _ctx.FileGroups.FindAsync(key);
            if (entity == null) return NotFound();
            _fileService.DeleteFileGroup(entity);
            await _ctx.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);

        }

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}