using Serilog;
using YumaIdentity.API.Extensions;
using YumaIdentity.API.Middleware;
using YumaIdentity.Application;
using YumaIdentity.Infrastructure;
using YumaIdentity.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithJwtAuthentication();

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(policy =>
{
    policy.SetIsOriginAllowed(origin =>
    {
        using var scope = app.Services.CreateScope();
        var corsService = scope.ServiceProvider.GetRequiredService<DynamicCorsService>();
        return corsService.IsOriginAllowedAsync(origin).GetAwaiter().GetResult();
    })
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();
});

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.SeedDatabaseAsync();

app.Run();
