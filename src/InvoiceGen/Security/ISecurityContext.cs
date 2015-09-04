using DavidLievrouw.InvoiceGen.Domain.DTO;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface ISecurityContext {
    void SetAuthenticatedUser(User user);
    User GetAuthenticatedUser();
  }
}