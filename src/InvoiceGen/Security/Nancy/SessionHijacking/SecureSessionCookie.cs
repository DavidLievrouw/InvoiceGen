namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class SecureSessionCookie {
    public string SessionId { get; set; }
    public string Hash { get; set; }

    public bool IsSecured => !string.IsNullOrWhiteSpace(Hash);

    public override string ToString() {
      return SessionId +
             (IsSecured
               ? Hash
               : "");
    }
  }
}