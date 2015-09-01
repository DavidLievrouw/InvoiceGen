﻿using System;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using DavidLievrouw.InvoiceGen.Api.Models;
using InvoiceGen.Domain;
using Nancy;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen.Api {
  public class UsersModule : NancyModule {
    public UsersModule(
      INancyQueryHandler<GetCurrentUserRequest, User> getCurrentUserQueryHandler,
      INancyCommandHandler<LoginRequest> loginCommandHandler) {
      if (getCurrentUserQueryHandler == null) throw new ArgumentNullException(nameof(getCurrentUserQueryHandler));
      if (loginCommandHandler == null) throw new ArgumentNullException(nameof(loginCommandHandler));

      Get["api/user", true] = async (parameters, cancellationToken) => {
        this.RequiresAuthentication();
        return await getCurrentUserQueryHandler.Handle(this,
                                                       () => new GetCurrentUserRequest {
                                                         NancyContext = Context
                                                       });
      };

      Post["api/user/login", true] = async (parameters, cancellationToken) => await loginCommandHandler.Handle(this);
    }
  }
}