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

    public User AuthenticatedUser {
      get {
        var identity = _nancyContext.CurrentUser as InvoiceGenIdentity;
        return identity == null
          ? null
          : identity.User;
      }
      set { _nancyContext.CurrentUser = _invoiceGenIdentityFactory.Create(value); }
    }

    public ISession Session {
      get { return _sessionFromContextResolver.ResolveSession(_nancyContext); }
    }
  }
}