using DavidLievrouw.InvoiceGen.Security;

namespace DavidLievrouw.InvoiceGen.Api.Models {
  public class LogoutCommand {
    public ISecurityContext SecurityContext { get; set; }
  }
}