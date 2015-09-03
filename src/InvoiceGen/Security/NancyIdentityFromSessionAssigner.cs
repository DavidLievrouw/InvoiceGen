using System;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public class NancyIdentityFromSessionAssigner : INancyIdentityFromSessionAssigner {
    readonly ISessionFromContextResolver _sessionFromContextResolver;
    readonly IUserFromSessionResolver _userFromSessionResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public NancyIdentityFromSessionAssigner(
      ISessionFromContextResolver sessionFromContextResolver,
      IUserFromSessionResolver userFromSessionResolver,
      IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (sessionFromContextResolver == null) throw new ArgumentNullException("sessionFromContextResolver");
      if (userFromSessionResolver == null) throw new ArgumentNullException("userFromSessionResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _sessionFromContextResolver = sessionFromContextResolver;
      _userFromSessionResolver = userFromSessionResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public void AssignNancyIdentity(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");

      var session = _sessionFromContextResolver.ResolveSession(nancyContext);
      var userFromSession = _userFromSessionResolver.ResolveUser(session);
      nancyContext.CurrentUser = _invoiceGenIdentityFactory.Create(userFromSession);
    }
  }
}