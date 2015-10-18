using System;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class NancyIdentityFromContextAssigner : INancyIdentityFromContextAssigner {
    readonly INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    readonly IUserFromSessionResolver _userFromSessionResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public NancyIdentityFromContextAssigner(
      INancySessionFromNancyContextResolver nancySessionFromNancyContextResolver,
      IUserFromSessionResolver userFromSessionResolver,
      IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (nancySessionFromNancyContextResolver == null) throw new ArgumentNullException("nancySessionFromNancyContextResolver");
      if (userFromSessionResolver == null) throw new ArgumentNullException("userFromSessionResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _nancySessionFromNancyContextResolver = nancySessionFromNancyContextResolver;
      _userFromSessionResolver = userFromSessionResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public void AssignNancyIdentityFromContext(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");

      var session = _nancySessionFromNancyContextResolver.ResolveSession(nancyContext);
      var userFromSession = _userFromSessionResolver.ResolveUser(session);
      nancyContext.CurrentUser = _invoiceGenIdentityFactory.Create(userFromSession);
    }
  }
}