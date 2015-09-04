using DavidLievrouw.InvoiceGen.Domain.DTO;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public interface IInvoiceGenIdentityFactory {
    InvoiceGenIdentity Create(User user);
  }
}