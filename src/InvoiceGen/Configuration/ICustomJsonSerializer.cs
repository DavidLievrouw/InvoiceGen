namespace DavidLievrouw.InvoiceGen.Configuration {
  public interface ICustomJsonSerializer {
    string Serialize(object model);
    T Deserialize<T>(string reader);
  }
}