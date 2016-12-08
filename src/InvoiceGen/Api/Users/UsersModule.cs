using System;
using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.InvoiceGen.Security.Nancy;
using DavidLievrouw.Utils;
using Nancy;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen.Api.Users {
  public class UsersModule : NancyModule {
    public UsersModule(
      IHandler<GetCurrentUserRequest, User> getCurrentUserHandler,
      IHandler<LoginRequest, bool> loginHandler,
      IHandler<LogoutRequest, bool> logoutHandler,
      INancySecurityContextFactory nancySecurityContextFactory) {
      if (getCurrentUserHandler == null) throw new ArgumentNullException("getCurrentUserHandler");
      if (loginHandler == null) throw new ArgumentNullException("loginHandler");
      if (logoutHandler == null) throw new ArgumentNullException("logoutHandler");
      if (nancySecurityContextFactory == null) throw new ArgumentNullException("nancySecurityContextFactory");

      Get["api/user", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await getCurrentUserHandler.Handle(this.Bind(() =>
          new GetCurrentUserRequest {
            SecurityContext = nancySecurityContextFactory.Create(Context)
          }));
      };

      Post["api/user/login", true] = async (parameters, cancellationToken) => await loginHandler.Handle(this.Bind(() => {
        var loginRequest = this.Bind<LoginRequest>();
        return new LoginRequest {
          SecurityContext = nancySecurityContextFactory.Create(Context),
          Login = loginRequest?.Login,
          Password = loginRequest?.Password
        };
      }));

      Post["api/user/logout", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await logoutHandler.Handle(this.Bind(() =>
          new LogoutRequest {
            SecurityContext = nancySecurityContextFactory.Create(Context)
          }));
      };
    }
  }
}