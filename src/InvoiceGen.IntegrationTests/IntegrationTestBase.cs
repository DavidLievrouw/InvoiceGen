using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen {
  public class IntegrationTestBase {
    [SetUp]
    protected void BaseSetUp() {
      var prepper = new DatabasePrepper(DatabaseConfigurationReader.Read());
      DbConfig = prepper.PrepareDatabaseForCurrentTest();
    }

    [TearDown]
    protected void BaseTearDown() {
      var fileToDelete = DbConfig.DatabaseFile;
      fileToDelete.Delete();
    }

    public virtual DatabaseConfiguration DbConfig { get; set; }
  }
}