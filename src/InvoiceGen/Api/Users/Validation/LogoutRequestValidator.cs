using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Common;
using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Api.Users.Validation {
  public class LogoutRequestValidator : NullAllowableValidator<LogoutRequest> {
    public LogoutRequestValidator() {
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid Nancy context should be specified.");
    }
  }
}