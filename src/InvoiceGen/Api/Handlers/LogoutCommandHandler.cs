using System;
using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class LogoutCommandHandler : ICommandHandler<LogoutCommand> {
    readonly ISessionFromContextResolver _sessionFromContextResolver;

    public LogoutCommandHandler(ISessionFromContextResolver sessionFromContextResolver) {
      if (sessionFromContextResolver == null) throw new ArgumentNullException("sessionFromContextResolver");
      _sessionFromContextResolver = sessionFromContextResolver;
    }

    public Task Handle(LogoutCommand command) {
      // Abandon session
      var session = _sessionFromContextResolver.ResolveSession(command.NancyContext);
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