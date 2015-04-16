using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace tapStoryWebApi.Attributes
{
    public class ValidateViewModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.Any(kv => kv.Value == null))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Arguments cannot be null");
                return;
            }

            if (actionContext.ModelState.IsValid) return;
            var errors = (from state in actionContext.ModelState from error in state.Value.Errors select error.ErrorMessage ?? ((error.Exception == null) ? "" : error.Exception.ToString())).ToList();
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
        }
    }
}