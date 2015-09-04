using DavidLievrouw.InvoiceGen.Common;
using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  public class GetCurrentUserRequestValidator : NullAllowableValidator<GetCurrentUserRequest>, IGetCurrentUserRequestValidator {
    public GetCurrentUserRequestValidator() {
      RuleFor(req => req.SecurityContext)
        .NotNull()
        .WithMessage("A valid security context should be specified.");
    }
  }
}