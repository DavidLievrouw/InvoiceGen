using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class ValidationAwareQueryHandler<TArg, TResult> : IQueryHandler<TArg, TResult> {
    readonly IValidator<TArg> _validator;
    readonly IQueryHandler<TArg, TResult> _innerHandler;

    public ValidationAwareQueryHandler(IValidator<TArg> validator, IQueryHandler<TArg, TResult> innerHandler) {
      if (validator == null) throw new ArgumentNullException("validator");
      if (innerHandler == null) throw new ArgumentNullException("innerHandler");
      _validator = validator;
      _innerHandler = innerHandler;
    }

    public async Task<TResult> Handle(TArg query) {
      _validator.ValidateAndThrow(query);
      return await _innerHandler.Handle(query);
    }
  }
}