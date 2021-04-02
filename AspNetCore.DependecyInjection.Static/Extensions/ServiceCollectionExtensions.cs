using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.DependecyInjection.Static.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedStatic<TService>(this IServiceCollection serviceCollection)
            where TService : class
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
            return serviceCollection;
        }
        
        public static IServiceCollection AddScopedStatic<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TService : class
            where TImplementation : class, TService
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
            return serviceCollection;
        }
        
        public static IServiceCollection AddTransientStatic<TService>(this IServiceCollection serviceCollection)
            where TService : class
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
            return serviceCollection;
        }
        
        public static IServiceCollection AddTransientStatic<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TService : class
            where TImplementation : class, TService
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
            return serviceCollection;
        }
        
        public static IServiceCollection AddSingletonStatic<TService>(this IServiceCollection serviceCollection)
            where TService : class
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
            return serviceCollection;
        }

        public static IServiceCollection AddSingletonStatic<TService, TImplementation>(this IServiceCollection serviceCollection)
            where TService : class
            where TImplementation : class, TService
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
            return serviceCollection;
        }
    }
}