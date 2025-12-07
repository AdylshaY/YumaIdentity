using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YumaIdentity.Application.Interfaces;

namespace YumaIdentity.API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireTenantAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentUserService = context.HttpContext.RequestServices.GetService<ICurrentUserService>();

            if (string.IsNullOrEmpty(currentUserService?.ClientId))
            {
                context.Result = new UnauthorizedObjectResult(new { Message = "Client context is required for this action." });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
