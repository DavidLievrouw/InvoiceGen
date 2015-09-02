using Nancy;

namespace DavidLievrouw.InvoiceGen.Api.Models {
  public class LoginCommand {
    public string Login { get; set; }
    public string Password { get; set; }
    public NancyContext NancyContext { get; set; }
  }
}