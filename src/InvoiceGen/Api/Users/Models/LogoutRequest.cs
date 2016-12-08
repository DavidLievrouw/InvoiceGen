using DavidLievrouw.InvoiceGen.Security;

namespace DavidLievrouw.InvoiceGen.Api.Users.Models {
  public class LogoutRequest {
    public ISecurityContext SecurityContext { get; set; }
  }
}