using DavidLievrouw.InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public interface IInvoiceGenIdentityFactory {
    InvoiceGenIdentity Create(User user);
  }
}