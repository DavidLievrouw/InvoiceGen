using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen {
  public class IntegrationTestBase {
    [SetUp]
    protected void BaseSetUp() {
      DbConfig = DatabaseConfigurationReader.Read();
    }

    public virtual DatabaseConfiguration DbConfig { get; set; }
  }
}