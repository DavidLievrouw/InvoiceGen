using System;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public class AuthenticatedUserApplyerFactory : IAuthenticatedUserApplyerFactory {
    readonly ISessionFromContextResolver _sessionFromContextResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public AuthenticatedUserApplyerFactory(ISessionFromContextResolver sessionFromContextResolver, IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (sessionFromContextResolver == null) throw new ArgumentNullException(nameof(sessionFromContextResolver));
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException(nameof(invoiceGenIdentityFactory));
      _sessionFromContextResolver = sessionFromContextResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public IAuthenticatedUserApplyer Create(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException(nameof(nancyContext));
      return new AuthenticatedUserApplyer(nancyContext, _sessionFromContextResolver, _invoiceGenIdentityFactory);
    }
  }
}