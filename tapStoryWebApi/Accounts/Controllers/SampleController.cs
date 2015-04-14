using System.Web.Http;

namespace tapStoryWebApi.Accounts.Controllers
{
    [Authorize]
    public class SampleController : ApiController
    {

        public IHttpActionResult Get()
        {
            return Ok("Hello");
        }

    }
}