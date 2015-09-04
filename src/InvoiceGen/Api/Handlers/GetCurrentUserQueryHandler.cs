using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserRequest, User> {
    public Task<User> Handle(GetCurrentUserRequest request) {
      return Task.FromResult(request.SecurityContext.GetAuthenticatedUser());
    }
  }
}