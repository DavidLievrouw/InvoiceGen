using DavidLievrouw.InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class InvoiceGenIdentityFactory : IInvoiceGenIdentityFactory {
    public InvoiceGenIdentity Create(User user) {
      return user == null
        ? null
        : new InvoiceGenIdentity(user);
    }
  }
}