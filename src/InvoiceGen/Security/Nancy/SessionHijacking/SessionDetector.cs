using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class SessionDetector : ISessionDetector {
    public bool IsInSession(Request request) {
      string dummy;
      return request.Cookies.TryGetValue(MemoryCacheBasedSessions.CookieName, out dummy);
    }
  }
}