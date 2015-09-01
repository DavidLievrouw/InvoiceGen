using System;
using System.Threading.Tasks;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public interface INancyQueryHandler<out TResponse> {
    Task<dynamic> Handle(INancyModule module);
  }

  public interface INancyQueryHandler<in TRequest, out TResponse> {
    Task<dynamic> Handle(INancyModule module);
    Task<dynamic> Handle(INancyModule module, Func<TRequest> bindFunc);
  }
}