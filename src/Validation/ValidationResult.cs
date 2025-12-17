using NDB.Abstraction.Results;

namespace NDB.Abstraction.Validation;

public class ValidationResult : Result
{
    public IReadOnlyList<ValidationError> Errors { get; init; }
        = Array.Empty<ValidationError>();
}
