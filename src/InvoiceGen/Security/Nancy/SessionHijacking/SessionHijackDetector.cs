using System;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class SessionHijackDetector : ISessionHijackDetector {
    readonly ISessionDetector _sessionDetector;
    readonly ISecureSessionCookieReader _cookieReader;
    readonly ISessionAntiHijackHashGenerator _hashGenerator;

    public SessionHijackDetector(
      ISessionDetector sessionDetector,
      ISecureSessionCookieReader cookieReader,
      ISessionAntiHijackHashGenerator hashGenerator) {
      if (sessionDetector == null) throw new ArgumentNullException(nameof(sessionDetector));
      if (cookieReader == null) throw new ArgumentNullException(nameof(cookieReader));
      if (hashGenerator == null) throw new ArgumentNullException(nameof(hashGenerator));
      _sessionDetector = sessionDetector;
      _cookieReader = cookieReader;
      _hashGenerator = hashGenerator;
    }

    public bool IsSessionHijacked(Request request) {
      if (!_sessionDetector.IsInSession(request)) return false;
      var secureCookie = _cookieReader.Read(request);

      return (secureCookie == null || !secureCookie.IsSecured || secureCookie.Hash != _hashGenerator.GenerateHash(request));
    }
  }
}