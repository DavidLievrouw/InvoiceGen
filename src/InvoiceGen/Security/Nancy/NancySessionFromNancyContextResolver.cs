using System;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class NancySessionFromNancyContextResolver : INancySessionFromNancyContextResolver {
    public ISession ResolveSession(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException(nameof(nancyContext));

      return nancyContext.Request?.Session == null
        ? null
        : new NancySession(nancyContext.Request.Session);
    }
  }
}