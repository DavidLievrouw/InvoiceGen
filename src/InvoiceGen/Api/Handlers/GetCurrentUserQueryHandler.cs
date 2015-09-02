using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserRequest, User> {
    public Task<User> Handle(GetCurrentUserRequest request) {
      var identity = request.NancyContext.CurrentUser as InvoiceGenIdentity;
      return Task.FromResult(identity == null ? null : identity.User);
    }
  }
}