using System.Collections.Immutable;
using System.Linq;
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
    private const string RefithanceWebAssemblyName = "Refithance.Web";

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

        var interfacesToRegisterWithCompilation = context.CompilationProvider.Combine(interfacesToRegister);
        context.RegisterImplementationSourceOutput(interfacesToRegisterWithCompilation,
            static (spc, source) => GenerateSource(source, spc)
        );
    }

    private static void GenerateSource((Compilation compilation, ImmutableArray<RefithanceInfo> refithanceInfos) source,
        SourceProductionContext context)
    {
        var (compilation, refithanceInfos) = source;
        var compilationIncludesRefithanceWeb = compilation.ReferencedAssemblyNames.Any(
            a => a.Name.Contains(RefithanceWebAssemblyName)
        );

        context.AddSource(SourceGeneratorHelpers.ServiceCollectionExtensionsFileName,
            SourceText.From(
                SourceGeneratorHelpers.GenerateServiceCollectionExtensionsClass(refithanceInfos,
                    compilationIncludesRefithanceWeb
                ),
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
