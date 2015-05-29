using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace tapStoryWebApi.Common.Exceptions.Global
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var requestContext = context.RequestContext;
            var config = requestContext.Configuration;

            context.Result = new ErrorResult(
              context.Exception,
              requestContext.IncludeErrorDetail,
              config.Services.GetContentNegotiator(),
              context.Request,
              config.Formatters);
        }

        /// An implementation of IHttpActionResult interface.
        private class ErrorResult : ExceptionResult
        {
            public ErrorResult(
              Exception exception,
              bool includeErrorDetail,
              IContentNegotiator negotiator,
              HttpRequestMessage request,
              IEnumerable<MediaTypeFormatter> formatters) :
                base(exception, includeErrorDetail, negotiator, request, formatters)
            {
            }

            /// Creates an HttpResponseMessage instance asynchronously.
            /// This method determines how a HttpResponseMessage content will look like.
            public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var errorId = Exception.Data["tapStory:id"] ?? String.Empty;
                
                var content = new HttpError(Exception, IncludeErrorDetail)
                {
                    {"ErrorId", errorId.ToString()}
                };

                // define an additional content field with name "ErrorID"

                var result =
                  ContentNegotiator.Negotiate(typeof(HttpError), Request, Formatters);

                var message = new HttpResponseMessage
                {
                    RequestMessage = Request,
                    StatusCode = result == null ?
                      HttpStatusCode.NotAcceptable : HttpStatusCode.InternalServerError
                };

                if (result != null)
                {
                    try
                    {
                        // serializes the HttpError instance either to JSON or to XML
                        // depend on requested by the client MIME type.
                        message.Content = new ObjectContent<HttpError>(
                          content,
                          result.Formatter,
                          result.MediaType);
                    }
                    catch
                    {
                        message.Dispose();

                        throw;
                    }
                }

                return Task.FromResult(message);
            }
        }

    }
}