using System;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.InvoiceGen.Security.AspNet;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class NancySecurityContext : ISecurityContext {
    readonly NancyContext _nancyContext;
    readonly IAspNetSessionFromNancyContextResolver _aspNetSessionFromNancyContextResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public NancySecurityContext(NancyContext nancyContext, IAspNetSessionFromNancyContextResolver sessionFromNancyContextResolver, IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");
      if (sessionFromNancyContextResolver == null) throw new ArgumentNullException("sessionFromNancyContextResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _nancyContext = nancyContext;
      _aspNetSessionFromNancyContextResolver = sessionFromNancyContextResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public void SetAuthenticatedUser(User user) {
      var session = _aspNetSessionFromNancyContextResolver.ResolveSession(_nancyContext);
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