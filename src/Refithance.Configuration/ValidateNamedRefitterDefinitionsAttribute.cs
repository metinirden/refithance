using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
using Refithance.Configuration.Validations;

namespace Refithance.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public class ValidateNamedRefithanceDefinitionsAttribute : ValidationAttribute
{
    private const string NotSupportedMessage =
        $"{nameof(ValidateNamedRefithanceDefinitionsAttribute)} can only be used for {nameof(Dictionary<string, RefithanceDefinition>)}.";

    private const string FailedValidationResultMessage = "Definitions validation failed.";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IDictionary<string, RefithanceDefinition> httpClientDefinitions)
        {
            throw new NotSupportedException(NotSupportedMessage);
        }

        var optionsResultBuilder = new ValidateOptionsResultBuilder();
        var validator = new ValidateRefithanceDefinition();

        foreach (var (name, definition) in httpClientDefinitions)
        {
            var validationResult = validator.Validate(name, definition);
            optionsResultBuilder.AddResult(validationResult);
        }

        var validateOptionsResult = optionsResultBuilder.Build();

        if (validateOptionsResult.Failed)
        {
            return new ValidationResult(FailedValidationResultMessage, validateOptionsResult.Failures);
        }

        return ValidationResult.Success;
    }
}
