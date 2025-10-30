using YumaIdentity.API.Extensions;
using YumaIdentity.API.Middleware;
using YumaIdentity.Application;
using YumaIdentity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithJwtAuthentication();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        policy => policy.WithOrigins("http://localhost:3000")
//                        .AllowAnyHeader()
//                        .AllowAnyMethod());
//});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.SeedDatabaseAsync();

app.Run();
