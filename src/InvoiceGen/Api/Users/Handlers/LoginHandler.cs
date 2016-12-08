using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Users.Handlers {
  public class LoginHandler : IHandler<LoginRequest, bool> {
    public Task<bool> Handle(LoginRequest request) {
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

      request.SecurityContext.SetAuthenticatedUser(user);

      return Task.FromResult(true);
    }
  }
}