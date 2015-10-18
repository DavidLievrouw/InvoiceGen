using System.Web;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class SecureSessionCookieReader : ISecureSessionCookieReader {
    const int RealSessionIdLength = 36;

    public SecureSessionCookie Read(Request request) {
      string combinedCookieValue;
      if (!request.Cookies.TryGetValue(MemoryCacheBasedSessions.CookieName, out combinedCookieValue)) return null;

      return combinedCookieValue.Length <= RealSessionIdLength
        ? new SecureSessionCookie {
          SessionId = combinedCookieValue,
          Hash = null
        }
        : new SecureSessionCookie {
          SessionId = combinedCookieValue.Substring(0, RealSessionIdLength),
          Hash = HttpUtility.UrlDecode(combinedCookieValue.Substring(RealSessionIdLength))
        };
    }
  }
}