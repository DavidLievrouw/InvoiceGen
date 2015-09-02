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
      if (sessionFromContextResolver == null) throw new ArgumentNullException(nameof(sessionFromContextResolver));
      if (userFromSessionResolver == null) throw new ArgumentNullException(nameof(userFromSessionResolver));
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException(nameof(invoiceGenIdentityFactory));
      _sessionFromContextResolver = sessionFromContextResolver;
      _userFromSessionResolver = userFromSessionResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public void AssignNancyIdentity(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException(nameof(nancyContext));

      var session = _sessionFromContextResolver.ResolveSession(nancyContext);
      var userFromSession = _userFromSessionResolver.ResolveUser(session);
      nancyContext.CurrentUser = _invoiceGenIdentityFactory.Create(userFromSession);
    }
  }
}