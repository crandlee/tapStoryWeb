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
    public class BooksController : ODataController
    {
        private ApplicationDbContext _ctx;
        private BookService _bookService;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            _ctx = ServiceFactory.GetDbContext<ApplicationDbContext>(controllerContext.Request);
            _bookService = ServiceFactory.GetBookService(_ctx, controllerContext.Request, controllerContext.RequestContext.Principal);
            base.Initialize(controllerContext);
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_bookService.GetBookFileGroups());
            }
            catch (Exception e)
            {
                return new InternalErrorActionResult(this, e, "Get");
            }
        }

    }
}