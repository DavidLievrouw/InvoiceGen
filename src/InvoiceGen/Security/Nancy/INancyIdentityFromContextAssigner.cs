using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public interface INancyIdentityFromContextAssigner {
    void AssignNancyIdentityFromContext(NancyContext nancyContext);
  }
}