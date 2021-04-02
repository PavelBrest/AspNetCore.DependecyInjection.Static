using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace AspNetCore.DependecyInjection.Static
{
    public class SyntaxReceiver : ISyntaxContextReceiver
    {
        public List<IMethodSymbol> List { get; } = new();
        
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is not InvocationExpressionSyntax invocationExpressionSyntax ||
                context.SemanticModel.GetOperation(invocationExpressionSyntax) is not IInvocationOperation invocationOperation)
                return;

            if (invocationOperation.Type is null || 
                !invocationOperation.Type.ToDisplayString().Equals("Microsoft.Extensions.DependencyInjection.IServiceCollection"))
                return;

            var methodSymbol = invocationOperation.TargetMethod;
            
            if (!methodSymbol.IsGenericMethod ||
                !methodSymbol.IsExtensionMethod ||
                !methodSymbol.ToDisplayString().StartsWith("AspNetCore.DependecyInjection.Static.Extensions.ServiceCollectionExtensions"))
                return;
            
            List.Add(methodSymbol);
        }
    }
}