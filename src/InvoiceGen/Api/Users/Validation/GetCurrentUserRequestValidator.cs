using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Common;
using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Api.Users.Validation {
  public class GetCurrentUserRequestValidator : NullAllowableValidator<GetCurrentUserRequest> {
    public GetCurrentUserRequestValidator() {
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid security context should be specified.");
    }
  }
}