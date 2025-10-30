namespace YumaIdentity.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Infrastructure.Persistence;
    using YumaIdentity.Infrastructure.Services;

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

            // TODO (Bir sonraki adımda): 
            // IEmailService'i ve ITokenGenerator'ı buraya ekleyeceğiz.
            // services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            // services.AddScoped<IEmailService, SendGridEmailService>();

            // Ayar (Settings) sınıflarını da burada kaydedebiliriz
            // services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            // services.Configure<SendGridSettings>(configuration.GetSection("SendGrid"));

            return services;
        }
    }
}
