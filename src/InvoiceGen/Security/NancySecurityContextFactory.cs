using System;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public class NancySecurityContextFactory : ISecurityContextFactory {
    readonly ISessionFromContextResolver _sessionFromContextResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public NancySecurityContextFactory(ISessionFromContextResolver sessionFromContextResolver, IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (sessionFromContextResolver == null) throw new ArgumentNullException("sessionFromContextResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _sessionFromContextResolver = sessionFromContextResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public ISecurityContext Create(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");
      return new NancySecurityContext(nancyContext, _sessionFromContextResolver, _invoiceGenIdentityFactory);
    }
  }
}