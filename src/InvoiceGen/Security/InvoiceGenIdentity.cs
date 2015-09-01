using System;
using System.Collections.Generic;
using System.Linq;
using DavidLievrouw.InvoiceGen.Domain;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen.Security {
  public class InvoiceGenIdentity : IUserIdentity {
    public InvoiceGenIdentity(User user) {
      if (user == null) throw new ArgumentNullException(nameof(user));
      User = user;
    }

    public User User { get; }

    public string UserName => User.Login.Value;

    public IEnumerable<string> Claims => Enumerable.Empty<string>();
  }
}