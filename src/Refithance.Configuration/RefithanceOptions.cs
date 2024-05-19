using System.Collections.Generic;

namespace Refithance.Configuration;

public class RefithanceOptions
{
    public const string SectionPath = "Refithance";

    [ValidateNamedRefithanceDefinitions]
    public Dictionary<string, RefithanceDefinition> Definitions { get; set; } = new();

    public RefithanceDefinition Get(string name)
    {
        if (Definitions.TryGetValue(name, out var refithanceDefinition))
        {
            return refithanceDefinition!;
        }

        throw new KeyNotFoundException($"{name} not found in Refithance Definitions!");
    }
}
