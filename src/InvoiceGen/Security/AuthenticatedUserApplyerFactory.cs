using System;

namespace DavidLievrouw.InvoiceGen.Security {
  public class AuthenticatedUserApplyerFactory : IAuthenticatedUserApplyerFactory {
    public IAuthenticatedUserApplyer Create(ISecurityContext securityContext) {
      if (securityContext == null) throw new ArgumentNullException("securityContext");
      return new AuthenticatedUserApplyer(securityContext);
    }
  }
}