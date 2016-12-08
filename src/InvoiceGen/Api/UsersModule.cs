using System;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.InvoiceGen.Security.Nancy;
using DavidLievrouw.Utils;
using Nancy;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen.Api {
  public class UsersModule : NancyModule {
    public UsersModule(
      IHandler<GetCurrentUserRequest, User> getCurrentUserQueryHandler,
      IHandler<LoginCommand, bool> loginCommandHandler,
      IHandler<LogoutCommand, bool> logoutCommandHandler,
      INancySecurityContextFactory nancySecurityContextFactory) {
      if (getCurrentUserQueryHandler == null) throw new ArgumentNullException("getCurrentUserQueryHandler");
      if (loginCommandHandler == null) throw new ArgumentNullException("loginCommandHandler");
      if (logoutCommandHandler == null) throw new ArgumentNullException("logoutCommandHandler");
      if (nancySecurityContextFactory == null) throw new ArgumentNullException("nancySecurityContextFactory");

      Get["api/user", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await getCurrentUserQueryHandler.Handle(this.Bind(() =>
          new GetCurrentUserRequest {
            SecurityContext = nancySecurityContextFactory.Create(Context)
          }));
      };

      Post["api/user/login", true] = async (parameters, cancellationToken) => await loginCommandHandler.Handle(this.Bind(() => {
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
      }));

      Post["api/user/logout", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await logoutCommandHandler.Handle(this.Bind(() =>
          new LogoutCommand {
            SecurityContext = nancySecurityContextFactory.Create(Context)
          }));
      };
    }
  }
}