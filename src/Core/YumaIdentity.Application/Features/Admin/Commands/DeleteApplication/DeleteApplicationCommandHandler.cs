namespace YumaIdentity.Application.Features.Admin.Commands.DeleteApplication
{
    using MediatR;
    using System.Threading.Tasks;
    using YumaIdentity.Application.Common.Exceptions;
    using YumaIdentity.Application.Interfaces;

    public class DeleteApplicationCommandHandler : IRequestHandler<DeleteApplicationCommand, Unit>
    {
        private readonly IAppDbContext _context;

        public DeleteApplicationCommandHandler(IAppDbContext context)
        {
            _context = context;
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

            return Unit.Value;
        }
    }
}
