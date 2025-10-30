using YumaIdentity.Application;
using YumaIdentity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// TODO (Bir sonraki adým): 
// AuthService.Application katmaný için de bir DI kurulumu yapacaðýz.
// builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: If a client-side application is to be added to the dashboard, its address must be entered here.
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        policy => policy.WithOrigins("http://localhost:3000")
//                        .AllowAnyHeader()
//                        .AllowAnyMethod());
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
