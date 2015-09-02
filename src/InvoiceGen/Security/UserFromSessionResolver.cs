using System;
using DavidLievrouw.InvoiceGen.Domain;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public class UserFromSessionResolver : IUserFromSessionResolver {
    readonly ISessionResolver _sessionResolver;

    public UserFromSessionResolver(ISessionResolver sessionResolver) {
      if (sessionResolver == null) throw new ArgumentNullException("sessionResolver");
      _sessionResolver = sessionResolver;
    }

    public User ResolveUser(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");

      var session = _sessionResolver.ResolveSession(nancyContext);
      return session?["user"] as User;
    }
  }
}