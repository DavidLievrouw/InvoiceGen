using DavidLievrouw.InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface IInvoiceGenIdentityFactory {
    InvoiceGenIdentity Create(User user);
  }
}