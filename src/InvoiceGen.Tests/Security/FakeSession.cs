using System;
using System.Collections.Generic;

namespace DavidLievrouw.InvoiceGen.Security {
  public class FakeSession : ISession {
    readonly Dictionary<string, object> _items;

    public FakeSession() {
      _items = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
    }

    public void CheckAbandoned() {
      if (IsAbandoned) throw new InvalidOperationException("The session has been abandoned.");
    }

    public bool IsAbandoned { get; set; }

    public object this[string name] {
      get {
        return _items.ContainsKey(name)
          ? _items[name]
          : null;
      }
      set {
        CheckAbandoned();
        _items[name] = value;
      }
    }

    public void Abandon() {
      IsAbandoned = true;
    }
  }
}