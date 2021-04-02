using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AspNetCore.DependecyInjection.Static
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
                return;

            var builder = new ExtensionModuleBuilder();

            foreach (var methodSymbol in receiver.List)
                builder.AddRegistration(methodSymbol, ref context);

            var sourceCode = builder.Build();
            context.AddSource("StaticModule_gen", SourceText.From(sourceCode, Encoding.UTF8));
        }
    }
}