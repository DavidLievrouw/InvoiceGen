using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.AspNet {
  public interface IAspNetSessionFromNancyContextResolver {
    ISession ResolveSession(NancyContext nancyContext);
  }
}