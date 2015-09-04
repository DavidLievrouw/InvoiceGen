using System;
using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class LogoutCommandHandler : ICommandHandler<LogoutCommand> {
    readonly IAuthenticatedUserApplyerFactory _authenticatedUserApplyerFactory;

    public LogoutCommandHandler(IAuthenticatedUserApplyerFactory authenticatedUserApplyerFactory) {
      if (authenticatedUserApplyerFactory == null) throw new ArgumentNullException("authenticatedUserApplyerFactory");
      _authenticatedUserApplyerFactory = authenticatedUserApplyerFactory;
    }

    public Task Handle(LogoutCommand command) {
      var applyer = _authenticatedUserApplyerFactory.Create(command.SecurityContext);
      applyer.ClearAuthenticatedUser();

      return Task.CompletedTask;
    }
  }
}