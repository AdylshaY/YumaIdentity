namespace YumaIdentity.Application.Features.Auth.Commands.RegisterUser
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Application.Models;
    using YumaIdentity.Domain.Entities;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserRequest, Guid>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(
            IAppDbContext context,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Guid> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.ClientId == request.ClientId, cancellationToken);

            if (application == null) throw new NotFoundException("Application", request.ClientId);

            var userExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (userExists) throw new ValidationException("Email already in use.");

            var defaultRole = await _context.AppRoles
                .FirstOrDefaultAsync(r => r.ApplicationId == application.Id && r.RoleName == "User", cancellationToken);

            if (defaultRole == null) throw new ValidationException("Default 'User' role not configured for this application. Please seed the database.");

            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                HashedPassword = hashedPassword,
                IsEmailVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = defaultRole.Id
            };

            _context.Users.Add(user);
            _context.UserRoles.Add(userRole);

            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
