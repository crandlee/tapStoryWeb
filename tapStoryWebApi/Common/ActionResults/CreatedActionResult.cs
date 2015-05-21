using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace tapStoryWebApi.Common.ActionResults
{
    public class CreatedActionResult<T> : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly string _location;
        private readonly T _returnObject;

        public CreatedActionResult(HttpRequestMessage request, string location, T returnObject) : this(request, location)
        {
            _returnObject = returnObject;
        }

        public CreatedActionResult(HttpRequestMessage request, string location)
        {
            _request = request;
            _location = location;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.Created, _returnObject);
            response.Headers.Location = new Uri(_location);
            return Task.FromResult(response);
        }
    }
}