namespace YumaIdentity.Application.Features.Admin.Commands.DeleteApplication
{
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;

    public class DeleteApplicationCommandHandler : IRequestHandler<DeleteApplicationCommand, Unit>
    {
        private const string ValidAudiencesCacheKey = "ValidAudiences";
        private readonly IAppDbContext _context;
        private readonly IMemoryCache _cache;

        public DeleteApplicationCommandHandler(IAppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Unit> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications.FindAsync(
                [request.Id],
                cancellationToken);

            if (application == null)
                throw new NotFoundException("Application", request.Id);

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Remove(ValidAudiencesCacheKey);

            return Unit.Value;
        }
    }
}
