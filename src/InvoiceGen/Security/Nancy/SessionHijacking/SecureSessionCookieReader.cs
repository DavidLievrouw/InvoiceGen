using System;
using System.Web;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class SecureSessionCookieReader : ISecureSessionCookieReader {
    const int RealSessionIdLength = 45;

    public SecureSessionCookie Read(Request request, string cookieName) {
      if (string.IsNullOrEmpty(cookieName)) throw new ArgumentNullException(nameof(cookieName));

      string combinedCookieValue;
      if (!request.Cookies.TryGetValue(cookieName, out combinedCookieValue)) return null;

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