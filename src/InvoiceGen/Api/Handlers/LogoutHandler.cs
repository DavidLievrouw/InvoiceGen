using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class LogoutHandler : IHandler<LogoutCommand, bool> {
    public Task<bool> Handle(LogoutCommand command) {
      command.SecurityContext.SetAuthenticatedUser(null);
      return Task.FromResult(true);
    }
  }
}