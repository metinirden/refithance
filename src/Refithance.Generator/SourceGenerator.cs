using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Refithance.Generator;

[Generator]
internal sealed class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(
            ctx => ctx.AddSource(SourceGeneratorHelpers.AttributeFileName,
                SourceText.From(SourceGeneratorHelpers.Attribute, Encoding.UTF8)
            )
        );
    }
}
