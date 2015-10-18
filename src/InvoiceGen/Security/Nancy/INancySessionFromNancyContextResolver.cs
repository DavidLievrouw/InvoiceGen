using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public interface INancySessionFromNancyContextResolver {
    ISession ResolveSession(NancyContext nancyContext);
  }
}