using System;
using DavidLievrouw.InvoiceGen.Security.AspNet;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class NancySecurityContextFactory : INancySecurityContextFactory {
    readonly IAspNetSessionFromNancyContextResolver _aspNetSessionFromNancyContextResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public NancySecurityContextFactory(IAspNetSessionFromNancyContextResolver sessionFromNancyContextResolver, IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (sessionFromNancyContextResolver == null) throw new ArgumentNullException("sessionFromNancyContextResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _aspNetSessionFromNancyContextResolver = sessionFromNancyContextResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public ISecurityContext Create(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");
      return new NancySecurityContext(nancyContext, _aspNetSessionFromNancyContextResolver, _invoiceGenIdentityFactory);
    }
  }
}