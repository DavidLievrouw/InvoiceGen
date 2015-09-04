using DavidLievrouw.InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface ISecurityContext {
    void SetAuthenticatedUser(User user);
    User GetAuthenticatedUser();
  }
}