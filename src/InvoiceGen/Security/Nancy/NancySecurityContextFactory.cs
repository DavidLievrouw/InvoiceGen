using System;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class NancySecurityContextFactory : INancySecurityContextFactory {
    readonly INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public NancySecurityContextFactory(INancySessionFromNancyContextResolver nancySessionFromNancyContextResolver, IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (nancySessionFromNancyContextResolver == null) throw new ArgumentNullException("nancySessionFromNancyContextResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _nancySessionFromNancyContextResolver = nancySessionFromNancyContextResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public ISecurityContext Create(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");
      return new NancySecurityContext(nancyContext, _nancySessionFromNancyContextResolver, _invoiceGenIdentityFactory);
    }
  }
}