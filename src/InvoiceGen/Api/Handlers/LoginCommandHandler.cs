using System;
using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class LoginCommandHandler : ICommandHandler<LoginRequest> {
    readonly ISessionResolver _sessionResolver;
    readonly IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;

    public LoginCommandHandler(ISessionResolver sessionResolver, IInvoiceGenIdentityFactory invoiceGenIdentityFactory) {
      if (sessionResolver == null) throw new ArgumentNullException(nameof(sessionResolver));
      if (invoiceGenIdentityFactory == null) throw new ArgumentNullException(nameof(invoiceGenIdentityFactory));
      _sessionResolver = sessionResolver;
      _invoiceGenIdentityFactory = invoiceGenIdentityFactory;
    }

    public Task Handle(LoginRequest command) {
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

      // Set user in session variable
      var session = _sessionResolver.ResolveSession(command.NancyContext);
      if (session != null) {
        session["user"] = user;
      }

      // Set user in broka identity
      command.NancyContext.CurrentUser = _invoiceGenIdentityFactory.Create(user);

      return Task.CompletedTask;
    }
  }
}