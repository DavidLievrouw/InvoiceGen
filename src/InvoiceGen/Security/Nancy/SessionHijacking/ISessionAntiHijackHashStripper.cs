using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public interface ISessionAntiHijackHashStripper {
    void StripHashFromCookie(Request request);
  }
}