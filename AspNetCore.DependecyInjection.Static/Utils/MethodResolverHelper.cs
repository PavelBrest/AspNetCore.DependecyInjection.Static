using System;
using Microsoft.CodeAnalysis;

namespace AspNetCore.DependecyInjection.Static.Utils
{
    internal static class MethodResolverHelper
    {
        public static (INamedTypeSymbol service, INamedTypeSymbol implementation) GetGenericTypeSymbols(IMethodSymbol methodSymbol)
        {
            if (methodSymbol is null) throw new ArgumentNullException(nameof(methodSymbol));
            
            var genericArgsArray = methodSymbol.TypeArguments;
            var serviceTypeSymbol = genericArgsArray[0];
            var implementationTypeSymbol = genericArgsArray.Length == 2
                ? genericArgsArray[1]
                : serviceTypeSymbol;
            
            return ((INamedTypeSymbol)serviceTypeSymbol, (INamedTypeSymbol)implementationTypeSymbol);
        }

        public static string GetServiceCollectionMethodName(IMethodSymbol methodSymbol)
        {
            if (methodSymbol is null) throw new ArgumentNullException(nameof(methodSymbol));

            var methodName = methodSymbol.ToDisplayParts()[10].ToString();
            return methodName.Remove(methodName.Length - "Static".Length);
        }
    }
}