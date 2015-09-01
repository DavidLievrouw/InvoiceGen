using System;
using InvoiceGen.Common;
using InvoiceGen.Domain;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public class UserFromSessionResolver : IUserFromSessionResolver {
    public User ResolveUser(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");

      return nancyContext
        .GetHttpContext()
        .Get(httpContextBase => httpContextBase.Session)
        .Get(session => session["user"] as User);
    }
  }
}