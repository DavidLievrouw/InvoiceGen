using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Users.Handlers {
  public class GetCurrentUserHandler : IHandler<GetCurrentUserRequest, User> {
    public Task<User> Handle(GetCurrentUserRequest request) {
      return Task.FromResult(request.SecurityContext.GetAuthenticatedUser());
    }
  }
}