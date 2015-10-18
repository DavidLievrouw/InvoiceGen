using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public interface ISessionAntiHijackHashInjector {
    void InjectHashInCookie(NancyContext context);
  }
}