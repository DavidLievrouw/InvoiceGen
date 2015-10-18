using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public interface IAntiSessionHijackLogic {
    Response InterceptHijackedSession(Request request);
    void ProtectResponseFromSessionHijacking(NancyContext context);
  }
}