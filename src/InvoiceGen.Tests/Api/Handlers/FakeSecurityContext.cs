using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.InvoiceGen.Security;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class FakeSecurityContext : ISecurityContext {
    public User AuthenticatedUser { get; set; }
    public ISession Session { get; set; }
  }
}