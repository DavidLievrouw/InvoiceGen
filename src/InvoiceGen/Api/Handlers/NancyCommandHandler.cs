using System;
using System.Threading.Tasks;
using DavidLievrouw.Utils;
using FluentValidation;
using Nancy;
using Nancy.ModelBinding;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class NancyCommandHandler<TRequest> : INancyCommandHandler<TRequest> {
    readonly ICommandHandler<TRequest> _handler;

    public NancyCommandHandler(ICommandHandler<TRequest> handler) {
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
        await _handler.Handle(request);
        return module.Negotiate.WithStatusCode(HttpStatusCode.OK);
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