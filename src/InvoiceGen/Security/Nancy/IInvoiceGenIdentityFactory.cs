using DavidLievrouw.InvoiceGen.Domain.DTO;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public interface IInvoiceGenIdentityFactory {
    IUserIdentity Create(User user);
  }
}