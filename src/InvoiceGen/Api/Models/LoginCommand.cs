using DavidLievrouw.InvoiceGen.Security;

namespace DavidLievrouw.InvoiceGen.Api.Models {
  public class LoginCommand {
    public string Login { get; set; }
    public string Password { get; set; }
    public ISecurityContext SecurityContext { get; set; }
  }
}