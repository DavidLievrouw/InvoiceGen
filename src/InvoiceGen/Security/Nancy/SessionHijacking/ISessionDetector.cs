using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public interface ISessionDetector {
    bool IsInSession(Request request);
  }
}