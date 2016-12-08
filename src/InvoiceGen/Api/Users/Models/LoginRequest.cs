using DavidLievrouw.InvoiceGen.Security;

namespace DavidLievrouw.InvoiceGen.Api.Users.Models {
  public class LoginRequest {
    public string Login { get; set; }
    public string Password { get; set; }
    public ISecurityContext SecurityContext { get; set; }
  }
}