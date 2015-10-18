using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public interface ISecureSessionCookieReader {
    SecureSessionCookie Read(Request request);
  }
}