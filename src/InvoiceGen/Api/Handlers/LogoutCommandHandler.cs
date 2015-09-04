using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class LogoutCommandHandler : ICommandHandler<LogoutCommand> {
    public Task Handle(LogoutCommand command) {
      command.SecurityContext.SetAuthenticatedUser(null);
      return Task.CompletedTask;
    }
  }
}