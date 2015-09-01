using System;
using System.Web;
using DavidLievrouw.Utils;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public class SessionResolver : ISessionResolver {
    public HttpSessionStateBase ResolveSession(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");

      return nancyContext
        .GetHttpContext()
        .Get(httpContextBase => httpContextBase.Session);
    }
  }
}