using System;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class SessionAntiHijackHashStripper : ISessionAntiHijackHashStripper {
    readonly ISessionDetector _sessionDetector;
    readonly ISecureSessionCookieReader _secureSessionCookieReader;

    public SessionAntiHijackHashStripper(ISessionDetector sessionDetector, ISecureSessionCookieReader secureSessionCookieReader) {
      if (sessionDetector == null) throw new ArgumentNullException(nameof(sessionDetector));
      if (secureSessionCookieReader == null) throw new ArgumentNullException(nameof(secureSessionCookieReader));
      _sessionDetector = sessionDetector;
      _secureSessionCookieReader = secureSessionCookieReader;
    }

    public void StripHashFromCookie(Request request) {
      if (!_sessionDetector.IsInSession(request)) return;

      var secureCookie = _secureSessionCookieReader.Read(request);
      if (secureCookie == null) return;

      request.Cookies[MemoryCacheBasedSessions.CookieName] = secureCookie.SessionId;
    }
  }
}