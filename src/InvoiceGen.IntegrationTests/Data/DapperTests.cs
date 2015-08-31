using System.Linq;
using DavidLievrouw.InvoiceGen.Data.Dapper;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Data {
  [TestFixture]
  public class DapperTests : IntegrationTestBase {
    IDbConnectionFactory _dbConnectionFactory;
    IQueryExecutor<IDbConnectionFactory> _executor;

    [SetUp]
    public void SetUp() {
      _dbConnectionFactory = new DbConnectionFactoryByConnectionString(DbConfig.ConnectionStringSettings);
      _executor = new QueryExecutor<IDbConnectionFactory>(_dbConnectionFactory);
    }

    public class BasicCRUDTests : DapperTests {
      [Test]
      public void CanExecuteSelectQuery() {
        const string query = @"
          SELECT IL.Country AS CountryCode, C.CountryName, IL.Year, IL.Month, COUNT(IL.ID) AS NumInvoices, SUM(IL.TotalRate) AS TotalRate, SUM(IL.NumTreatments) AS TotalNumTreatments
          FROM q_InvoiceList AS IL INNER JOIN Country AS C ON C.Code = IL.Country
          GROUP BY IL.Country, C.CountryName, IL.Year, IL.Month;";
        var actualLoadedRecords = _executor.NewQuery(query).ExecuteWithAnonymousResult(new {
          CountryCode = default(string),
          CountryName = default(string),
          Year = default(short),
          Month = default(short),
          NumInvoices = default(int),
          TotalRate = default(decimal),
          TotalNumTreatments = default(double)
        });
        Assert.That(actualLoadedRecords, Is.Not.Null);
        Assert.That(actualLoadedRecords.Count(), Is.GreaterThan(0));
      }

      [Test]
      public void CanExecuteInsertQuery() {
        const string query = @"INSERT INTO Country (Code, CountryName) VALUES (@Code, @CountryName)";
        var actualNumRecordsInserted = _executor.NewQuery(query)
                                                .WithParameters(new {
                                                  Code = "AM",
                                                  CountryName = "Armenia"
                                                })
                                                .Execute();
        Assert.That(actualNumRecordsInserted, Is.GreaterThan(0));
      }

      [Test]
      public void CanExecuteUpdateQuery() {
        const string insertQuery = @"INSERT INTO Country (Code, CountryName) VALUES (@Code, @CountryName)";
        var actualNumRecordsInserted = _executor.NewQuery(insertQuery)
                                                .WithParameters(new {
                                                  Code = "MI",
                                                  CountryName = "Mali"
                                                })
                                                .Execute();
        Assert.That(actualNumRecordsInserted, Is.GreaterThan(0));

        const string deleteQuery = @"UPDATE Country SET Code=@Code WHERE CountryName=@CountryName";
        var actualNumRecordsDeleted = _executor.NewQuery(deleteQuery)
                                               .WithParameters(new {
                                                 Code = "ML",
                                                 CountryName = "Mali"
                                               })
                                               .Execute();
        Assert.That(actualNumRecordsDeleted, Is.GreaterThan(0));
      }

      [Test]
      public void CanExecuteDeleteQuery() {
        const string insertQuery = @"INSERT INTO Country (Code, CountryName) VALUES (@Code, @CountryName)";
        var actualNumRecordsInserted = _executor.NewQuery(insertQuery)
                                                .WithParameters(new {
                                                  Code = "KY",
                                                  CountryName = "Cayman Islands"
                                                })
                                                .Execute();
        Assert.That(actualNumRecordsInserted, Is.GreaterThan(0));

        const string deleteQuery = @"DELETE FROM Country WHERE Code=@Code";
        var actualNumRecordsDeleted = _executor.NewQuery(deleteQuery)
                                               .WithParameters(new {
                                                 Code = "KY"
                                               })
                                               .Execute();
        Assert.That(actualNumRecordsDeleted, Is.GreaterThan(0));
      }
    }
  }
}