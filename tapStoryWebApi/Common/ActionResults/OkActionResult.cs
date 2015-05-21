using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace tapStoryWebApi.Common.ActionResults
{
    public class OkActionResult<T> : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly T _returnObject;

        public OkActionResult(HttpRequestMessage request, T returnObject)
            : this(request)
        {
            _returnObject = returnObject;
        }

        public OkActionResult(HttpRequestMessage request)
        {
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.OK, _returnObject);
            return Task.FromResult(response);
        }
    }
}