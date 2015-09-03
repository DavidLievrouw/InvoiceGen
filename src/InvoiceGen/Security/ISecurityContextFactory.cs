using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface ISecurityContextFactory {
    ISecurityContext Create(NancyContext nancyContext);
  }
}