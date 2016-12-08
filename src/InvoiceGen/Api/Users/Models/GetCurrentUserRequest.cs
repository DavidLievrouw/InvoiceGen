using DavidLievrouw.InvoiceGen.Security;

namespace DavidLievrouw.InvoiceGen.Api.Users.Models {
  public class GetCurrentUserRequest {
    public ISecurityContext SecurityContext { get; set; }
  }
}