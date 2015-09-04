using DavidLievrouw.InvoiceGen.Domain.DTO;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface IUserFromSessionResolver {
    User ResolveUser(ISession session);
  }
}