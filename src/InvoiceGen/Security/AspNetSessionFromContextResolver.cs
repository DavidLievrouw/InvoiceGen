using System;
using DavidLievrouw.Utils;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public class AspNetSessionFromContextResolver : ISessionFromContextResolver {
    public ISession ResolveSession(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");

      var aspNetSession = nancyContext
        .GetHttpContext()
        .Get(httpContextBase => httpContextBase.Session);

      return aspNetSession == null
        ? null
        : new AspNetSession(aspNetSession);
    }
  }
}