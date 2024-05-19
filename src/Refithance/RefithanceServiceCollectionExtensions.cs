using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Refithance.Configuration;
using Refithance.Configuration.Validations;

namespace Refithance;

public static class RefithanceServiceCollectionExtensions2
{
    public static void AddRefithanceValidation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IValidateOptions<RefithanceOptions>, ValidateRefithanceOptions>();
        serviceCollection.AddSingleton<IValidateOptions<RefithanceDefinition>, ValidateRefithanceDefinition>();
        serviceCollection.AddOptionsWithValidateOnStart<RefithanceOptions>().BindConfiguration(
            RefithanceOptions.SectionPath
        );
    }

    public static void UseRefithanceValidationWatcher(this IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        ChangeToken.OnChange(
            () => configuration.GetSection(RefithanceOptions.SectionPath).GetReloadToken(),
            sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RefithanceOptions>>();
                try
                {
                    var optionsFactory = sp.GetRequiredService<IOptionsFactory<RefithanceOptions>>();
                    _ = optionsFactory.Create(Options.DefaultName);
                    logger.LogInformation("RefithanceOptions successfully changed!");
                }
                catch (OptionsValidationException optionsValidationException)
                {
                    logger.LogCritical(optionsValidationException, optionsValidationException.Message);
                }
            },
            serviceProvider
        );
    }
}
