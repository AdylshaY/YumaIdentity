namespace YumaIdentity.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Infrastructure.Options;
    using YumaIdentity.Infrastructure.Persistence;
    using YumaIdentity.Infrastructure.Services;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using System.Security.Claims;

    public static class DependencyInjectionExt
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddSingleton<IValidAudienceService, DatabaseAudienceService>();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

            var jwtSettings = new JwtSettings();
            configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            var serviceProvider = services.BuildServiceProvider();
            var audienceService = serviceProvider.GetRequiredService<IValidAudienceService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateLifetime = true,
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,
                    ValidateAudience = true,
                    AudienceValidator = (audiences, securityToken, validationParameters) =>
                    {
                        return audienceService.IsAudienceValidAsync(audiences).GetAwaiter().GetResult();
                    }
                };
            });

            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            return services;
        }
    }
}
