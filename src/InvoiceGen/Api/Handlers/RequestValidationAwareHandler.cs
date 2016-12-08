using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class RequestValidationAwareHandler<TRequest, TResponse> : IHandler<TRequest, TResponse> {
    readonly IHandler<TRequest, TResponse> _innerHandler;
    readonly IValidator<TRequest> _validator;

    public RequestValidationAwareHandler(IValidator<TRequest> validator, IHandler<TRequest, TResponse> innerHandler) {
      if (validator == null) throw new ArgumentNullException(nameof(validator));
      if (innerHandler == null) throw new ArgumentNullException(nameof(innerHandler));
      _validator = validator;
      _innerHandler = innerHandler;
    }

    public async Task<TResponse> Handle(TRequest request) {
      ValidationResult validationResult;
      try {
        validationResult = _validator.Validate(request);
      }
      catch (Exception ex) {
        throw new RequestValidationException(ex);
      }

      if (!validationResult.IsValid) throw new RequestValidationException(validationResult, $"Request validation failed ({typeof(TRequest).Name}).");
      return await _innerHandler.Handle(request);
    }
  }
}