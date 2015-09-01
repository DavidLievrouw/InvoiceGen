using FluentValidation;
using InvoiceGen.Common;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  public class GetCurrentUserRequestValidator : NullAllowableValidator<GetCurrentUserRequest>, IGetCurrentUserRequestValidator {
    public GetCurrentUserRequestValidator() {
      RuleFor(req => req.NancyContext)
        .NotNull()
        .WithMessage("A valid Nancy context should be specified.");
    }
  }
}