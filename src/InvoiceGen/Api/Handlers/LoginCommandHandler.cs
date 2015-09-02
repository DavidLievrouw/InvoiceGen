using System;
using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class LoginCommandHandler : ICommandHandler<LoginCommand> {
    readonly IAuthenticatedUserApplyerFactory _authenticatedUserApplyerFactory;

    public LoginCommandHandler(IAuthenticatedUserApplyerFactory authenticatedUserApplyerFactory) {
      if (authenticatedUserApplyerFactory == null) throw new ArgumentNullException(nameof(authenticatedUserApplyerFactory));
      _authenticatedUserApplyerFactory = authenticatedUserApplyerFactory;
    }

    public Task Handle(LoginCommand command) {
      // Authorise user
      var user = new User {
        GivenName = "John",
        LastName = "Doe",
        Login = new Login {Value = "JDoe"},
        Password = new Password {
          Value = "P@$$w0rd",
          IsEncrypted = false
        }
      };

      var applyer = _authenticatedUserApplyerFactory.Create(command.NancyContext);
      if (user == null) {
        applyer.ClearAuthenticatedUser();
      } else {
        applyer.ApplyAuthenticatedUser(user);
      }

      return Task.CompletedTask;
    }
  }
}