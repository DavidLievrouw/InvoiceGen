using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class GetCurrentUserHandler : IHandler<GetCurrentUserRequest, User> {
    public Task<User> Handle(GetCurrentUserRequest request) {
      return Task.FromResult(request.SecurityContext.GetAuthenticatedUser());
    }
  }
}