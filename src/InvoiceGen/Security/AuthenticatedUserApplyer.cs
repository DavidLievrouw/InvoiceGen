using System;
using DavidLievrouw.InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Security {
  public class AuthenticatedUserApplyer : IAuthenticatedUserApplyer {
    readonly ISecurityContext _securityContext;

    public AuthenticatedUserApplyer(ISecurityContext securityContext) {
      if (securityContext == null) throw new ArgumentNullException("securityContext");
      _securityContext = securityContext;
    }

    public void ApplyAuthenticatedUser(User user) {
      if (user == null) throw new ArgumentNullException("user");

      var session = _securityContext.Session;
      if (session == null) throw new InvalidOperationException("There is no current session.");

      session["user"] = user;
      _securityContext.AuthenticatedUser = user;
    }

    public void ClearAuthenticatedUser() {
      var session = _securityContext.Session;
      if (session != null) {
        session["user"] = null;
        session.Abandon();
      }
      _securityContext.AuthenticatedUser = null;
    }
  }
}