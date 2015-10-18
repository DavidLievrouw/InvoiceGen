using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public interface IResponseBuilderWhenSessionIsHijacked {
    Response BuildHijackedResponse();
  }
}