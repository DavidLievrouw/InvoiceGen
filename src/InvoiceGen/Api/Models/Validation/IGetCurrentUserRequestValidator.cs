using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  public interface IGetCurrentUserRequestValidator : IValidator<GetCurrentUserRequest> {}
}