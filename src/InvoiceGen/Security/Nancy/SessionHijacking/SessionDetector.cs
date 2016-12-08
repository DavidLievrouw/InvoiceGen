using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class SessionDetector : ISessionDetector {
    public bool IsInSession(Request request) {
      string dummy;
      // ToDo: use real cookie name
      return request.Cookies.TryGetValue("_nsid", out dummy);
    }
  }
}