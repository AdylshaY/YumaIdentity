namespace YumaIdentity.Application
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using YumaIdentity.Application.Common.Interfaces.Mediator;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

            foreach (var type in handlerTypes)
            {
                var interfaceType = type.GetInterfaces().First(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

                services.AddTransient(interfaceType, type);
            }

            return services;
        }
    }
}
