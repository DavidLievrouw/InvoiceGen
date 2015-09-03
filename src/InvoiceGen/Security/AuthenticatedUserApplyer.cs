using System;
using DavidLievrouw.InvoiceGen.Domain;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public class AuthenticatedUserApplyer : IAuthenticatedUserApplyer {
    readonly NancyContext _nancyContext;
    readonly ISessionFromContextResolver _sessionFromContextResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public AuthenticatedUserApplyer(NancyContext nancyContext, ISessionFromContextResolver sessionFromContextResolver, IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");
      if (sessionFromContextResolver == null) throw new ArgumentNullException("sessionFromContextResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _nancyContext = nancyContext;
      _sessionFromContextResolver = sessionFromContextResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public void ApplyAuthenticatedUser(User user) {
      if (user == null) throw new ArgumentNullException("user");

      var session = _sessionFromContextResolver.ResolveSession(_nancyContext);
      if (session == null) throw new InvalidOperationException("There is no current session.");

      session["user"] = user;
      _nancyContext.CurrentUser = _invoiceGenIdentityFactory.Create(user);
    }

    public void ClearAuthenticatedUser() {
      var session = _sessionFromContextResolver.ResolveSession(_nancyContext);
      if (session != null) {
        session["user"] = null;
        session.Abandon();
      }
      _nancyContext.CurrentUser = null;
    }
  }
}