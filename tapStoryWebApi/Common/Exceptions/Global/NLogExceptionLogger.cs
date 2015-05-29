using System;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace tapStoryWebApi.Common.Exceptions.Global
{
    public class NLogExceptionLogger: ExceptionLogger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void Log(ExceptionLoggerContext context)
        {
            try
            {
                var request = context.Request;
                var exception = context.Exception;
                var id = Guid.NewGuid();
                Logger.Error("A global exception was thrown - {0} ** {1} ** {2} ** {3} ** {4} ** {5}", id, request.RequestUri,  context.RequestContext == null ?
                    null : context.RequestContext.Principal.Identity.Name, request, exception.Message, exception.StackTrace);


                // associates retrieved error ID with the current exception
                exception.Data["tapStory:id"] = id;
            }
            catch
            {
                // logger shouldn't throw an exception!!!
            }

            
        }

    }
}