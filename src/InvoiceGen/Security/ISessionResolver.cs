using System.Web;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface ISessionResolver {
    HttpSessionStateBase ResolveSession(NancyContext nancyContext);
  }
}