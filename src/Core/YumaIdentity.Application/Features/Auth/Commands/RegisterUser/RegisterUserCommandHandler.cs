namespace YumaIdentity.Application.Features.Auth.Commands.RegisterUser
{
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;
    using YumaIdentity.Domain.Entities;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserRequest, Guid>
    {
        private readonly IAppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IClientValidator _clientValidator;

        public RegisterUserCommandHandler(
            IAppDbContext context,
            IPasswordHasher passwordHasher,
            IClientValidator clientValidator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _clientValidator = clientValidator;
        }

        public async Task<Guid> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var application = await _clientValidator.ValidateAndGetApplicationAsync(request.ClientId, request.ClientSecret);

            if (application == null) throw new NotFoundException("Application", request.ClientId);

            Guid? targetTenantId = application.IsIsolated ? application.Id : null;

            var userExists = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.TenantId == targetTenantId, cancellationToken);

            if (userExists != null)
            {
                if (!application.IsIsolated)
                {
                    bool hasRole = userExists.UserRoles.Any(ur => ur.Role.ApplicationId == application.Id);
                    if (hasRole) throw new ValidationException("User already registered for this application.");
                    throw new ValidationException("You have a Global Account. Please login to connect.");
                }

                throw new ValidationException("Email already in use in this application.");
            }

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
                CreatedAt = DateTime.UtcNow,
                TenantId = targetTenantId
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
