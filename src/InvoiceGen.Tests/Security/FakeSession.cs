using System;
using System.Collections.Generic;

namespace DavidLievrouw.InvoiceGen.Security {
  public class FakeSession : ISession {
    readonly Dictionary<string, object> _items;

    public FakeSession() {
      _items = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
    }

    public object this[string name] {
      get {
        return _items.ContainsKey(name)
          ? _items[name]
          : null;
      }
      set { _items[name] = value; }
    }

    public void Abandon() {}
  }
}