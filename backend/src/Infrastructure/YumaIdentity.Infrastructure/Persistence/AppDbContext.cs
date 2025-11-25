namespace YumaIdentity.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;

    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<TokenType> TokenTypes { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasIndex(e => e.ClientId).IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => new { e.Email, e.TenantId }).IsUnique();
            });

            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.HasIndex(e => new { e.ApplicationId, e.RoleName }).IsUnique();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(e => e.RoleId);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasIndex(e => e.TokenHash);
                entity.HasOne(e => e.Application)
                    .WithMany()
                    .HasForeignKey(e => e.ApplicationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TokenType>().HasData(
                new TokenType { Id = 1, TypeName = "EMAIL_VERIFY", Description = "Kullanıcının e-posta adresini doğrulaması için gönderilen token." },
                new TokenType { Id = 2, TypeName = "PASSWORD_RESET", Description = "Kullanıcının şifresini sıfırlaması için gönderilen token." }
            );
        }
    }
}
