using System.Collections.Generic;
using System.Linq;
using Nancy.Security;

namespace DavidLievrouw.InvoiceGen {
  public class FakeUserIdentity : IUserIdentity {
    public FakeUserIdentity(string userName) {
      UserName = userName;
    }

    public string UserName { get; private set; }

    public IEnumerable<string> Claims {
      get { return Enumerable.Empty<string>(); }
    }
  }
}