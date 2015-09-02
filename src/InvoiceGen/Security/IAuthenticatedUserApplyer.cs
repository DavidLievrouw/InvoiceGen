using DavidLievrouw.InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface IAuthenticatedUserApplyer {
    void ApplyAuthenticatedUser(User user);
    void ClearAuthenticatedUser();
  }
}