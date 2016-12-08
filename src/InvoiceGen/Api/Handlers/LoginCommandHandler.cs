using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class LoginCommandHandler : IHandler<LoginCommand, bool> {
    public Task<bool> Handle(LoginCommand command) {
      // Authorise user: ToDo
      var user = new User {
        GivenName = "John",
        LastName = "Doe",
        Login = new Login {Value = "JDoe"},
        Password = new Password {
          Value = "P@$$w0rd",
          IsEncrypted = false
        }
      };

      command.SecurityContext.SetAuthenticatedUser(user);

      return Task.FromResult(true);
    }
  }
}