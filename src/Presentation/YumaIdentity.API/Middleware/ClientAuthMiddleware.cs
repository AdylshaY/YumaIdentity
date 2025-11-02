using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using YumaIdentity.Application.Interfaces;

namespace YumaIdentity.API.Middleware
{
    public class ClientAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public ClientAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/auth"))
            {
                if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
                {
                    var authHeader = AuthenticationHeaderValue.Parse(value!);
                    if (authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase) && authHeader.Parameter != null)
                    {
                        var (ClientId, ClientSecret) = ParseBasicAuth(authHeader.Parameter);
                        context.Items["BasicAuthClientId"] = ClientId;
                        context.Items["BasicAuthClientSecret"] = ClientSecret;
                    }
                }
            }
            await _next(context);
        }

        private static (string ClientId, string ClientSecret) ParseBasicAuth(string authParameter)
        {
            var credentialBytes = Convert.FromBase64String(authParameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            return (credentials[0], credentials[1]);
        }
    }
}
