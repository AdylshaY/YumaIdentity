using System.Collections.Concurrent;
using YumaIdentity.Application.Common.Interfaces.Mediator;

namespace YumaIdentity.Infrastructure.Services
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, Type> _handlerTypes = new();

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var requestType = request.GetType();

            var handlerType = _handlerTypes.GetOrAdd(requestType, type =>
            {
                var responseType = typeof(TResponse);
                return typeof(IRequestHandler<,>).MakeGenericType(type, responseType);
            });

            var handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException($"Handler not found for request of type {requestType.Name}. Register your handlers in DI.");
            var method = handlerType.GetMethod("Handle");

            return await (Task<TResponse>)method!.Invoke(handler, [request, cancellationToken])!;
        }
    }
}
