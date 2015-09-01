using System;
using System.Threading.Tasks;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public interface INancyCommandHandler<in TRequest> {
    Task<dynamic> Handle(INancyModule module);
    Task<dynamic> Handle(INancyModule module, Func<TRequest> bindFunc);
  }
}