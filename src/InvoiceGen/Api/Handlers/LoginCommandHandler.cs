using System;
using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.Utils;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class LoginCommandHandler : ICommandHandler<LoginRequest> {
    public Task Handle(LoginRequest command) {
      throw new NotImplementedException();
    }
  }
}