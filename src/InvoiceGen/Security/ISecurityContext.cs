using DavidLievrouw.InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface ISecurityContext {
    User AuthenticatedUser { get; set; }
    ISession Session { get; }
  }
}