using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using NLog;

namespace tapStoryWebApi.Common.ActionResults
{
    public class InternalErrorActionResult : IHttpActionResult
    {
        private readonly ApiController _controller;
        private readonly Exception _exception;
        private readonly string _methodName;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public InternalErrorActionResult(ApiController c, Exception e, [CallerMemberName] string methodName = null)
        {
            _controller = c;
            _exception = e;
            _methodName = methodName;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var refNum = Guid.NewGuid();
            Logger.Error("{0}({1}): An exception was thrown - {2}", _methodName, refNum, _exception);

            var message = String.Format("Exception Reference Number: {0}", refNum);
            #if DEBUG
            message = _exception.ToString();
            #endif

            return Task.FromResult(_controller.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message, _exception));

        }
    }
}