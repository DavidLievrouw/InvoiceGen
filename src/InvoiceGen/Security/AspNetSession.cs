using System;
using System.Web;

namespace DavidLievrouw.InvoiceGen.Security {
  public class AspNetSession : ISession {
    readonly HttpSessionStateBase _wrappedSession;

    public AspNetSession(HttpSessionStateBase wrappedSession) {
      if (wrappedSession == null) throw new ArgumentNullException("wrappedSession");
      _wrappedSession = wrappedSession;
    }

    public object this[string name] {
      get { return _wrappedSession[name]; }
      set { _wrappedSession[name] = value; }
    }

    public void Abandon() {
      _wrappedSession.Abandon();
    }
  }
}