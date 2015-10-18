using System;
using System.Security;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class NancySecurityContext : ISecurityContext {
    readonly NancyContext _nancyContext;
    readonly INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public NancySecurityContext(NancyContext nancyContext, INancySessionFromNancyContextResolver nancySessionFromNancyContextResolver, IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");
      if (nancySessionFromNancyContextResolver == null) throw new ArgumentNullException("nancySessionFromNancyContextResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _nancyContext = nancyContext;
      _nancySessionFromNancyContextResolver = nancySessionFromNancyContextResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public void SetAuthenticatedUser(User user) {
      var session = _nancySessionFromNancyContextResolver.ResolveSession(_nancyContext);
      if (session == null) throw new SecurityException("There is no current session.");
      session[Constants.SessionKeyForUser] = user;
      _nancyContext.CurrentUser = _invoiceGenIdentityFactory.Create(user);
      if (user == null) session.Abandon();
    }

    public User GetAuthenticatedUser() {
      var identity = _nancyContext.CurrentUser as InvoiceGenIdentity;
      return identity?.User;
    }
  }
}