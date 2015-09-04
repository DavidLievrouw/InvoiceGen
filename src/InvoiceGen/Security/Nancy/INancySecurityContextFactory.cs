using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public interface INancySecurityContextFactory {
    ISecurityContext Create(NancyContext nancyContext);
  }
}