using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class ResponseBuilderWhenSessionIsHijacked : IResponseBuilderWhenSessionIsHijacked {
    public Response BuildHijackedResponse() {
      return new Response {
        StatusCode = HttpStatusCode.Unauthorized,
        ReasonPhrase = "Session hijacking detected."
      };
    }
  }
}