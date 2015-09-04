using DavidLievrouw.InvoiceGen.Security;
namespace DavidLievrouw.InvoiceGen.Api.Models {
  public class GetCurrentUserRequest {
    public ISecurityContext SecurityContext { get; set; }
  }
}