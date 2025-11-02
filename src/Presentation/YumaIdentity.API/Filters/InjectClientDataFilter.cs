using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YumaIdentity.Application.Interfaces;

namespace YumaIdentity.API.Filters
{
    public class InjectClientDataFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var clientId = context.HttpContext.Items["BasicAuthClientId"] as string;
            var clientSecret = context.HttpContext.Items["BasicAuthClientSecret"] as string;

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                context.Result = new UnauthorizedObjectResult(new { Title = "Client credentials missing or invalid." });
                return;
            }

            var dto = context.ActionArguments.Values
                .FirstOrDefault(v => v is IClientAuthenticatedRequest);

            if (dto != null)
            {
                var clientRequest = (IClientAuthenticatedRequest)dto;
                clientRequest.ClientId = clientId;
                clientRequest.ClientSecret = clientSecret;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context) { }
    }
}
