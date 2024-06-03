using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Refithance.Configuration;

namespace Refithance;

public static class RefithanceBuilderExtensions
{
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
