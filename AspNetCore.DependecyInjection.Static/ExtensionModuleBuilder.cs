using System.Text;
using AspNetCore.DependecyInjection.Static.Utils;
using Microsoft.CodeAnalysis;

namespace AspNetCore.DependecyInjection.Static
{
    public class ExtensionModuleBuilder
    {
        private readonly StringBuilder _builder = new();

        public ExtensionModuleBuilder()
        {
            _builder.Append(@"
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.DependecyInjection.Static
{
    public static class StaticModule
    {
        public static void Register(IServiceCollection serviceCollection)
        {
");
        }

        public ExtensionModuleBuilder AddRegistration(IMethodSymbol methodSymbol, ref GeneratorExecutionContext context)
        {
            var methodName = MethodResolverHelper.GetServiceCollectionMethodName(methodSymbol);
            var (serviceTypeSymbol, implementationTypeSymbol) = MethodResolverHelper.GetGenericTypeSymbols(methodSymbol);

            var ctorParameters = BuildCtor(implementationTypeSymbol, ref context);

            _builder.Append("            ");
            _builder.Append($"serviceCollection.{methodName}<{serviceTypeSymbol.ToDisplayString()},{implementationTypeSymbol.ToDisplayString()}>");
            _builder.AppendLine($"(provider => new {implementationTypeSymbol.ToDisplayString()}({ctorParameters}));");
            
            return this;
        }

        public string Build()
        {
            _builder.Append(@"
        }
    }
}");
            
            return _builder.ToString();
        }

        private static string BuildCtor(INamedTypeSymbol typeSymbol, ref GeneratorExecutionContext context)
        {
            if (typeSymbol.Constructors.IsEmpty)
                return string.Empty;

            if (typeSymbol.Constructors.Length > 1)
                //TODO: context raise build error
                return string.Empty;

            var ctorMethodSymbol = typeSymbol.Constructors[0];
            var builder = new StringBuilder();

            for (int i = 0; i < ctorMethodSymbol.Parameters.Length; i++)
            {
                var parameterTypeInfo = ctorMethodSymbol.Parameters[i];
                builder.Append($"provider.GetService<{parameterTypeInfo.ToDisplayString()}>()");

                if (i != ctorMethodSymbol.Parameters.Length - 1)
                    builder.Append(", ");
            }
            
            return builder.ToString();
        }
    }
}