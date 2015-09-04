using DavidLievrouw.InvoiceGen.Domain.DTO;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class InvoiceGenIdentityFactory : IInvoiceGenIdentityFactory {
    public IUserIdentity Create(User user) {
      return user == null
        ? null
        : new InvoiceGenIdentity(user);
    }
  }
}