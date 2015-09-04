namespace DavidLievrouw.InvoiceGen.Domain.DTO {
  public class User {
    public Login Login { get; set; }
    public string GivenName { get; set; }
    public string LastName { get; set; }
    public Password Password { get; set; }
  }
}