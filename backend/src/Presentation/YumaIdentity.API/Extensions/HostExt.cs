using YumaIdentity.Application.Interfaces;

namespace YumaIdentity.API.Extensions
{
    public static class HostExt
    {
        public static async Task SeedDatabaseAsync(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<IHost>>();

                try
                {
                    logger.LogInformation("Veritabanı seed işlemi başlatılıyor...");
                    var seeder = services.GetRequiredService<IDatabaseSeeder>();
                    await seeder.InitializeAsync();
                    logger.LogInformation("Veritabanı seed işlemi başarıyla tamamlandı.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Veritabanı seed işlemi sırasında bir hata oluştu.");
                    throw;
                }
            }
        }
    }
}
