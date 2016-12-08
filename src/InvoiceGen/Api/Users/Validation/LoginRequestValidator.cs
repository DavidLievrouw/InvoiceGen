using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Common;
using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Api.Users.Validation {
  public class LoginRequestValidator : NullAllowableValidator<LoginRequest> {
    public LoginRequestValidator() {
      RuleFor(req => req.Login)
        .NotNull()
        .WithMessage("A valid login should be specified.");
      RuleFor(req => req.Password)
        .NotNull()
        .WithMessage("A valid password should be specified.");
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid security context should be specified.");
    }
  }
}