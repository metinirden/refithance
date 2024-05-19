using System.ComponentModel.DataAnnotations;

namespace Refithance.Configuration;

public class RefithanceDefinition
{
    [Required, Url] public string BaseAddress { get; set; } = string.Empty;
    [Range(1, 600_000)] public int TimeoutInMs { get; set; } = 30_000;
}
