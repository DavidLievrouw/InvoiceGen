using Nancy;

namespace DavidLievrouw.InvoiceGen.Api.Models {
  public class LoginRequest {
    public string Login { get; set; }
    public string Password { get; set; }
    public NancyContext NancyContext { get; set; }
  }
}