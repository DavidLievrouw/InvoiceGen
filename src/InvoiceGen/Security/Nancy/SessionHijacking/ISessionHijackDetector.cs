using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public interface ISessionHijackDetector {
    bool IsSessionHijacked(Request request);
  }
}