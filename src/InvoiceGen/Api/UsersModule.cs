using System;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.InvoiceGen.Security.Nancy;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen.Api {
  public class UsersModule : NancyModule {
    public UsersModule(
      INancyQueryHandler<GetCurrentUserRequest, User> getCurrentUserQueryHandler,
      INancyQueryHandler<LoginCommand, bool> loginCommandHandler,
      INancyQueryHandler<LogoutCommand, bool> logoutCommandHandler,
      INancySecurityContextFactory nancySecurityContextFactory) {
      if (getCurrentUserQueryHandler == null) throw new ArgumentNullException("getCurrentUserQueryHandler");
      if (loginCommandHandler == null) throw new ArgumentNullException("loginCommandHandler");
      if (logoutCommandHandler == null) throw new ArgumentNullException("logoutCommandHandler");
      if (nancySecurityContextFactory == null) throw new ArgumentNullException("nancySecurityContextFactory");

      Get["api/user", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await getCurrentUserQueryHandler.Handle(this,
          () => new GetCurrentUserRequest {
            SecurityContext = nancySecurityContextFactory.Create(Context)
          });
      };

      Post["api/user/login", true] = async (parameters, cancellationToken) => await loginCommandHandler.Handle(this,
        () => {
          var loginRequest = this.Bind<LoginCommand>();
          return new LoginCommand {
            SecurityContext = nancySecurityContextFactory.Create(Context),
            Login = loginRequest == null
              ? null
              : loginRequest.Login,
            Password = loginRequest == null
              ? null
              : loginRequest.Password
          };
        });
      Post["api/user/logout", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await logoutCommandHandler.Handle(this,
          () => new LogoutCommand {
            SecurityContext = nancySecurityContextFactory.Create(Context)
          });
      };
    }
  }
}