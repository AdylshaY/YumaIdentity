namespace YumaIdentity.Infrastructure
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;
    using System.Text;
    using YumaIdentity.Application.Common.Interfaces.Mediator;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Infrastructure.Options;
    using YumaIdentity.Infrastructure.Persistence;
    using YumaIdentity.Infrastructure.Services;

    public static class DependencyInjectionExt
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IPkceService, PkceService>();
            services.AddSingleton<IOAuthSessionService, OAuthSessionService>();

            services.Configure<CorsSettings>(configuration.GetSection(CorsSettings.SectionName));
            services.AddScoped<DynamicCorsService>();

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

            var jwtSettings = new JwtSettings();
            configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            services.Configure<SmtpSettings>(configuration.GetSection(SmtpSettings.SectionName));
            services.AddTransient<IEmailService, SmtpEmailService>();

            var serviceProvider = services.BuildServiceProvider();

            services.AddScoped<IClientValidator, ClientValidator>();

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
                    ValidAudience = configuration["AdminSeed:AdminClientId"],
                };
            });

            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            services.AddTransient<IMediator, Mediator>();

            return services;
        }
    }
}
