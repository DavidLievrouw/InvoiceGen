using Nancy;

namespace DavidLievrouw.InvoiceGen.Api.Models {
  public class LogoutCommand {
    public NancyContext NancyContext { get; set; }
  }
}