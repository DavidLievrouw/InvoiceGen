using Nancy;

namespace DavidLievrouw.InvoiceGen.Api.Models {
  public class GetCurrentUserRequest {
    public NancyContext NancyContext { get; set; }
  }
}