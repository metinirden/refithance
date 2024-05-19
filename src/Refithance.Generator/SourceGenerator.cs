using System.Collections.Immutable;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Refithance.Generator;

[Generator]
internal sealed class SourceGenerator : IIncrementalGenerator
{
    private const string RefithanceAttribute = "Refithance.Generator.RefithanceAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(
            ctx => ctx.AddSource(SourceGeneratorHelpers.AttributeFileName,
                SourceText.From(SourceGeneratorHelpers.Attribute, Encoding.UTF8)
            )
        );

        var interfacesToRegister = context.SyntaxProvider.ForAttributeWithMetadataName(
            RefithanceAttribute,
            (node, _) => node is InterfaceDeclarationSyntax,
            transform: GetTypeInformationForGeneration
        ).Collect();

        context.RegisterImplementationSourceOutput(interfacesToRegister,
            static (spc, source) => GenerateSource(source, spc)
        );
    }

    private static void GenerateSource(ImmutableArray<RefithanceInfo> source, SourceProductionContext context)
    {
        context.AddSource(SourceGeneratorHelpers.ServiceCollectionExtensionsFileName,
            SourceText.From(
                SourceGeneratorHelpers.GenerateServiceCollectionExtensionsClass(new StringBuilder(), source),
                Encoding.UTF8
            )
        );
    }


    private static RefithanceInfo GetTypeInformationForGeneration(GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken)
    {
        return new RefithanceInfo(
            context.TargetSymbol.Name,
            context.TargetSymbol.ToDisplayString()
        );
    }
}
