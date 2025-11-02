using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace YumaIdentity.API.Filters
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo.DeclaringType!.GetCustomAttribute<AuthorizeAttribute>() != null ||
                               context.MethodInfo.GetCustomAttribute<AuthorizeAttribute>() != null;

            if (hasAuthorize)
            {
                operation.Security =
                [
                    new() {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            Array.Empty<string>()
                        }
                    }
                ];
            }

            var hasClientFilter = context.MethodInfo.DeclaringType!.GetCustomAttribute<InjectClientDataFilter>() != null ||
                                  context.MethodInfo.GetCustomAttribute<InjectClientDataFilter>() != null;

            if (hasClientFilter)
            {
                operation.Security =
                [
                    new() {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
                            },
                            Array.Empty<string>()
                        }
                    }
                ];
            }
        }
    }
}
