using System;
using DavidLievrouw.InvoiceGen.Domain;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public class NancySecurityContext : ISecurityContext {
    readonly NancyContext _nancyContext;
    readonly ISessionFromContextResolver _sessionFromContextResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public NancySecurityContext(NancyContext nancyContext, ISessionFromContextResolver sessionFromContextResolver, IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");
      if (sessionFromContextResolver == null) throw new ArgumentNullException("sessionFromContextResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _nancyContext = nancyContext;
      _sessionFromContextResolver = sessionFromContextResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public void SetAuthenticatedUser(User user) {
      var session = _sessionFromContextResolver.ResolveSession(_nancyContext);
      if (session == null) throw new InvalidOperationException("There is no current session.");
      session["IC_User"] = user;
      _nancyContext.CurrentUser = _invoiceGenIdentityFactory.Create(user);
    }

    public User GetAuthenticatedUser() {
      var identity = _nancyContext.CurrentUser as InvoiceGenIdentity;
      return identity == null
        ? null
        : identity.User;
    }
  }
}