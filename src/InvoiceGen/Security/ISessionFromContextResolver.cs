using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface ISessionFromContextResolver {
    ISession ResolveSession(NancyContext nancyContext);
  }
}