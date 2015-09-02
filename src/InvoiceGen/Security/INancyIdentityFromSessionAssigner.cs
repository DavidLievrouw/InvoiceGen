using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface INancyIdentityFromSessionAssigner {
    void AssignNancyIdentity(NancyContext nancyContext);
  }
}