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
using tapStoryWebApi.Common.Factories;
using tapStoryWebApi.Common.Helpers;
using tapStoryWebApi.Exceptions;
using tapStoryWebApi.Files.Services;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

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

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            try
            {
                return Ok(_fileService.GetFileGroup(key));
            }
            catch (Exception e)
            {
                return new InternalErrorActionResult(this, e, "Get");                
            }
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult GetFiles([FromODataUri] int key)
        {
            try
            {
                return Ok(_fileService.GetFiles(key));
            }
            catch (Exception e)
            {
                return new InternalErrorActionResult(this, e, "GetFiles");
            }
        }

        public async Task<IHttpActionResult> Post(FileGroup fg)
        {

            try {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                _fileService.AddFileGroup(fg);
                await _ctx.SaveChangesAsync();
                return Created(fg);
            } catch (Exception e)
            {
                return new InternalErrorActionResult(this, e, "Post");
            }


        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<FileGroup> fg)
        {
            try
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
            } catch (Exception e)
            {
                return new InternalErrorActionResult(this, e, "Patch");
            }


        }

        public async Task<IHttpActionResult> Put([FromODataUri] int key, FileGroup update)
        {
            try {
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
            } catch (Exception e)
            {
                return new InternalErrorActionResult(this, e, "Put");
            }

        }


        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try {
                var entity = await _ctx.FileGroups.FindAsync(key);
                if (entity == null) return NotFound();
                _fileService.DeleteFileGroup(entity);
                await _ctx.SaveChangesAsync();
                return StatusCode(HttpStatusCode.NoContent);
            } catch (Exception e)
            {
                return new InternalErrorActionResult(this, e, "Delete");
            }

        }


        public async void DeleteRef([FromODataUri] int key, string navigationProperty,
            [FromBody] Uri link)
        {
            ReferenceFunctionHelper.CallReferenceFunction("FileGroups", navigationProperty, ReferenceServiceFunctionType.Delete, _fileService, key, Request, link);
            await _ctx.SaveChangesAsync();
        }

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}