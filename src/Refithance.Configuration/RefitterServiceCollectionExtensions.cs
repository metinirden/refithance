using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refithance.Configuration.Validations;

namespace Refithance.Configuration;

public static class RefithanceServiceCollectionExtensions
{
    public static void AddRefithanceValidation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IValidateOptions<RefithanceOptions>, ValidateRefithanceOptions>();
        serviceCollection.AddSingleton<IValidateOptions<RefithanceDefinition>, ValidateRefithanceDefinition>();
        serviceCollection.AddOptionsWithValidateOnStart<RefithanceOptions>().BindConfiguration(
            RefithanceOptions.SectionPath
        );
    }
}
