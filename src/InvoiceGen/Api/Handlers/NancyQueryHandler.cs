using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using FluentValidation;
using Nancy;
using Nancy.ModelBinding;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class NancyQueryHandler<TResponse> : INancyQueryHandler<TResponse> {
    readonly IHandler<TResponse> _handler;

    public NancyQueryHandler(IHandler<TResponse> handler) {
      if (handler == null) throw new ArgumentNullException("handler");
      _handler = handler;
    }

    public async Task<dynamic> Handle(INancyModule module) {
      if (module == null) throw new ArgumentNullException("module");
      try {
        return await _handler.Handle();
      } catch (Exception) {
        return module.Negotiate
                     .WithStatusCode(HttpStatusCode.InternalServerError);
      }
    }
  }

  public class NancyQueryHandler<TRequest, TResponse> : INancyQueryHandler<TRequest, TResponse> {
    readonly IHandler<TRequest, TResponse> _handler;

    public NancyQueryHandler(
      IHandler<TRequest, TResponse> handler) {
      if (handler == null) throw new ArgumentNullException("handler");
      _handler = handler;
    }

    public async Task<dynamic> Handle(INancyModule module) {
      return await Handle(module, module.Bind<TRequest>);
    }

    public async Task<dynamic> Handle(INancyModule module, Func<TRequest> bindFunc) {
      if (module == null) throw new ArgumentNullException("module");
      if (bindFunc == null) throw new ArgumentNullException("bindFunc");

      TRequest request;
      try {
        request = bindFunc();
      } catch (Exception) {
        return module.Negotiate
                     .WithStatusCode(HttpStatusCode.BadRequest);
      }

      try {
        return await _handler.Handle(request);
      } catch (ValidationException) {
        return module.Negotiate
                     .WithStatusCode(HttpStatusCode.BadRequest);
      } catch (Exception) {
        return module.Negotiate
                     .WithStatusCode(HttpStatusCode.InternalServerError);
      }
    }
  }
}