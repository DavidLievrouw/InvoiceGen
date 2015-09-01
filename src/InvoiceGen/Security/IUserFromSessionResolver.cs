using InvoiceGen.Domain;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface IUserFromSessionResolver {
    User ResolveUser(NancyContext owinContext);
  }
}