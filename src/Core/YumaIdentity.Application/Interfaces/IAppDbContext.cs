namespace YumaIdentity.Application.Interfaces
{
    using System.Threading.Tasks;
    using YumaIdentity.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    internal interface IAppDbContext
    {
        DbSet<Application> Applications { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<AppRole> AppRoles { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<TokenType> TokenTypes { get; set; }
        DbSet<UserToken> UserTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
