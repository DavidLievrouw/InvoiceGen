using DavidLievrouw.InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface IUserFromSessionResolver {
    User ResolveUser(ISession session);
  }
}