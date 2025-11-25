using Microsoft.OpenApi.Models;
using YumaIdentity.API.Filters;

namespace YumaIdentity.API.Extensions
{
    public static class SwaggerExt
    {
        public static IServiceCollection AddSwaggerWithJwtAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = "Access Token (JWT) for user authentication."
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            return services;
        }
    }
}
