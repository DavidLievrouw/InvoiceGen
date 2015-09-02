using System;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen.Api {
  public class UsersModule : NancyModule {
    public UsersModule(
      INancyQueryHandler<GetCurrentUserRequest, User> getCurrentUserQueryHandler,
      INancyCommandHandler<LoginRequest> loginCommandHandler) {
      if (getCurrentUserQueryHandler == null) throw new ArgumentNullException("getCurrentUserQueryHandler");
      if (loginCommandHandler == null) throw new ArgumentNullException("loginCommandHandler");

      Get["api/user", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await getCurrentUserQueryHandler.Handle(this,
                                                       () => new GetCurrentUserRequest {
                                                         NancyContext = Context
                                                       });
      };

      Post["api/user/login", true] = async (parameters, cancellationToken) => await loginCommandHandler.Handle(this,
                                                                                                               () => {
                                                                                                                 var loginRequest = this.Bind<LoginRequest>();
                                                                                                                 return new LoginRequest {
                                                                                                                   NancyContext = Context,
                                                                                                                   Login = loginRequest?.Login,
                                                                                                                   Password = loginRequest?.Password
                                                                                                                 };
                                                                                                               });
    }
  }
}