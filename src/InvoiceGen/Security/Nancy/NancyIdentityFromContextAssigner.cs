using System;
using DavidLievrouw.InvoiceGen.Security.AspNet;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class NancyIdentityFromContextAssigner : INancyIdentityFromContextAssigner {
    readonly IAspNetSessionFromNancyContextResolver _aspNetSessionFromNancyContextResolver;
    readonly IUserFromSessionResolver _userFromSessionResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public NancyIdentityFromContextAssigner(
      IAspNetSessionFromNancyContextResolver sessionFromNancyContextResolver,
      IUserFromSessionResolver userFromSessionResolver,
      IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (sessionFromNancyContextResolver == null) throw new ArgumentNullException("sessionFromNancyContextResolver");
      if (userFromSessionResolver == null) throw new ArgumentNullException("userFromSessionResolver");
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException("invoiceGenIdentityFactory");
      _aspNetSessionFromNancyContextResolver = sessionFromNancyContextResolver;
      _userFromSessionResolver = userFromSessionResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public void AssignNancyIdentityFromContext(NancyContext nancyContext) {
      if (nancyContext == null) throw new ArgumentNullException("nancyContext");

      var session = _aspNetSessionFromNancyContextResolver.ResolveSession(nancyContext);
      var userFromSession = _userFromSessionResolver.ResolveUser(session);
      nancyContext.CurrentUser = _invoiceGenIdentityFactory.Create(userFromSession);
    }
  }
}