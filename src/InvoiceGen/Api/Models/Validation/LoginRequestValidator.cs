using FluentValidation;
using InvoiceGen.Common;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  public class LoginRequestValidator : NullAllowableValidator<LoginRequest>, ILoginRequestValidator {
    public LoginRequestValidator() {
      RuleFor(req => req.Login)
        .Must(login => login != null)
        .WithMessage("A valid login should be specified.");
      RuleFor(req => req.Password)
        .Must(pwd => pwd != null)
        .WithMessage("A valid password should be specified.");
    }
  }
}