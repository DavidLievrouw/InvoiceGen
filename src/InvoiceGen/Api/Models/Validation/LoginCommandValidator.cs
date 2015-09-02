using DavidLievrouw.InvoiceGen.Common;
using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  public class LoginCommandValidator : NullAllowableValidator<LoginCommand>, ILoginCommandValidator {
    public LoginCommandValidator() {
      RuleFor(req => req.Login)
        .NotNull()
        .WithMessage("A valid login should be specified.");
      RuleFor(req => req.Password)
        .NotNull()
        .WithMessage("A valid password should be specified.");
      RuleFor(req => req.NancyContext)
        .NotNull()
        .WithMessage("A valid Nancy context should be specified.");
    }
  }
}