using System;
using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class LogoutCommandHandler : ICommandHandler<LogoutCommand> {
    readonly ISessionResolver _sessionResolver;

    public LogoutCommandHandler(ISessionResolver sessionResolver) {
      if (sessionResolver == null) throw new ArgumentNullException("sessionResolver");
      _sessionResolver = sessionResolver;
    }

    public Task Handle(LogoutCommand command) {
      // Abandon session
      var session = _sessionResolver.ResolveSession(command.NancyContext);
      if (session != null) {
        session["user"] = null;
        session.Abandon();
      }

      // Clear user in broka identity
      command.NancyContext.CurrentUser = null;

      return Task.CompletedTask;
    }
  }
}