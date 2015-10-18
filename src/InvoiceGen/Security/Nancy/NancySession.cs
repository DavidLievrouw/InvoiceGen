using System;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  public class NancySession : ISession {
    readonly global::Nancy.Session.ISession _wrappedSession;

    public NancySession(global::Nancy.Session.ISession wrappedSession) {
      if (wrappedSession == null) throw new ArgumentNullException(nameof(wrappedSession));
      _wrappedSession = wrappedSession;
    }

    public object this[string name] {
      get { return _wrappedSession[name]; }
      set { _wrappedSession[name] = value; }
    }

    public void Abandon() {
      _wrappedSession.DeleteAll();
    }
  }
}