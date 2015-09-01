using System;
using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using DavidLievrouw.Utils;
using Nancy;
using Nancy.ModelBinding;

namespace DavidLievrouw.InvoiceGen.Api {
  public class FakeNancyQueryHandler<TRequest, TResponse> : INancyQueryHandler<TRequest, TResponse> {
    readonly IQueryHandler<TRequest, TResponse> _innerHandler;
    dynamic _result;
    int _callCount;

    public FakeNancyQueryHandler(IQueryHandler<TRequest, TResponse> innerHandler) {
      if (innerHandler == null) throw new ArgumentNullException("innerHandler");
      _innerHandler = innerHandler;
      _callCount = 0;
    }

    public async Task<dynamic> Handle(INancyModule module) {
      return await Handle(module, module.Bind<TRequest>);
    }

    public async Task<dynamic> Handle(INancyModule module, Func<TRequest> bindFunc) {
      if (module == null) throw new ArgumentNullException("module");
      if (bindFunc == null) throw new ArgumentNullException("bindFunc");

      _callCount++;

      TRequest request;
      try {
        request = bindFunc();
      } catch (Exception) {
        return module.Negotiate.WithStatusCode(HttpStatusCode.BadRequest);
      }

      await _innerHandler.Handle(request);

      return _result;
    }

    public void Returns(dynamic result) {
      _result = result;
    }

    public int GetCallCount() {
      return _callCount;
    }
  }

  public class FakeNancyQueryHandler<TResponse> : INancyQueryHandler<TResponse> {
    readonly IQueryHandler<TResponse> _innerHandler;
    dynamic _result;
    int _callCount;

    public FakeNancyQueryHandler(IQueryHandler<TResponse> innerHandler) {
      if (innerHandler == null) throw new ArgumentNullException("innerHandler");
      _innerHandler = innerHandler;
      _callCount = 0;
    }

    public async Task<dynamic> Handle(INancyModule module) {
      if (module == null) throw new ArgumentNullException("module");

      _callCount++;

      await _innerHandler.Handle();

      return _result;
    }

    public void Returns(dynamic result) {
      _result = result;
    }

    public int GetCallCount() {
      return _callCount;
    }
  }
}