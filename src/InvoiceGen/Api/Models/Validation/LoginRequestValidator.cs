using DavidLievrouw.InvoiceGen.Common;
using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  public class LoginRequestValidator : NullAllowableValidator<LoginRequest>, ILoginRequestValidator {
    public LoginRequestValidator() {
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