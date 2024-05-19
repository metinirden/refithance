using Microsoft.Extensions.Options;

namespace Refithance.Configuration.Validations;

[OptionsValidator]
public partial class ValidateRefithanceOptions : IValidateOptions<RefithanceOptions>;
