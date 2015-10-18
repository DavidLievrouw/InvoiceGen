using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public interface ISessionAntiHijackHashGenerator {
    string GenerateHash(Request request);
  }
}