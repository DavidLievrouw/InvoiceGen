using FluentValidation;
using FluentValidation.Results;

namespace DavidLievrouw.InvoiceGen.Common {
  public abstract class NullAllowableValidator<T> : AbstractValidator<T> {
    protected bool IsNullAllowed { get; set; }
    
    public override ValidationResult Validate(T instance) {
      if (IsNullAllowed) {
        return instance == null
          ? new ValidationResult()
          : base.Validate(instance);
      } else {
        return instance == null
          ? new ValidationResult(new[] { new ValidationFailure(typeof(T).Name, typeof(T).Name + " instance cannot be null.") })
          : base.Validate(instance);
      }
    }
  }
}