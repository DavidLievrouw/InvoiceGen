using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Users.Handlers {
  public class LogoutHandler : IHandler<LogoutRequest, bool> {
    public Task<bool> Handle(LogoutRequest request) {
      request.SecurityContext.SetAuthenticatedUser(null);
      return Task.FromResult(true);
    }
  }
}