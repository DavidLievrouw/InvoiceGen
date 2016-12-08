using System;
using System.Linq;
using Nancy;
using Nancy.Cookies;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class SessionAntiHijackHashInjector : ISessionAntiHijackHashInjector {
    readonly ISessionAntiHijackHashGenerator _hashGenerator;

    public SessionAntiHijackHashInjector(ISessionAntiHijackHashGenerator hashGenerator) {
      if (hashGenerator == null) throw new ArgumentNullException(nameof(hashGenerator));
      _hashGenerator = hashGenerator;
    }

    public void InjectHashInCookie(NancyContext context) {
      // ToDo: Get real cookie name
      // ToDo: Should not use SingleOrDefault
      var unsecureCookie = context.Response.Cookies.SingleOrDefault(c => c.Name == "_nsid");

      if (unsecureCookie != null) {
        context.Response.Cookies.Remove(unsecureCookie);

        var secureCookie = new SecureSessionCookie {
          SessionId = unsecureCookie.Value,
          Hash = _hashGenerator.GenerateHash(context.Request)
        };

        var replacementCookie = new NancyCookie(
          unsecureCookie.Name,
          secureCookie.ToString(),
          unsecureCookie.HttpOnly,
          unsecureCookie.Secure,
          unsecureCookie.Expires);
        context.Response.Cookies.Add(replacementCookie);
      }
    }
  }
}