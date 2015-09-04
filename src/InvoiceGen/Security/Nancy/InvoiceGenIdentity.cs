using System;
using System.Collections.Generic;
using System.Linq;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class InvoiceGenIdentity : IUserIdentity {
    public InvoiceGenIdentity(User user) {
      if (user == null) throw new ArgumentNullException("user");
      User = user;
    }

    public User User { get; private set; }

    public string UserName {
      get { return User.Login.Value; }
    }

    public IEnumerable<string> Claims {
      get { return Enumerable.Empty<string>(); }
    }
  }
}