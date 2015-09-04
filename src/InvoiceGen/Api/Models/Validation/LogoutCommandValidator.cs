using DavidLievrouw.InvoiceGen.Common;
using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  public class LogoutCommandValidator : NullAllowableValidator<LogoutCommand>, ILogoutCommandValidator {
    public LogoutCommandValidator() {
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid Nancy context should be specified.");
    }
  }
}