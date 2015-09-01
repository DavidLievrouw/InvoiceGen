using System;
using System.Threading.Tasks;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.Utils;
using InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserRequest, User> {
    public Task<User> Handle(GetCurrentUserRequest request) {
      throw new NotImplementedException();
    }
  }
}