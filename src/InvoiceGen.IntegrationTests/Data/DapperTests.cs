using System;
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

    [Test]
    public void CanExecuteSelectQuery() {
      const string query = @"
          SELECT IL.Country AS CountryCode, C.CountryName, IL.Year, IL.Month, COUNT(IL.ID) AS NumInvoices, SUM(IL.TotalRate) AS TotalRate, SUM(IL.NumTreatments) AS TotalNumTreatments
          FROM q_InvoiceList AS IL INNER JOIN Country AS C ON C.Code = IL.Country
          GROUP BY IL.Country, C.CountryName, IL.Year, IL.Month;";
      var actual = _executor.NewQuery(query).ExecuteWithAnonymousResult(new {
        CountryCode = default(string),
        CountryName = default(string),
        Year = default(short),
        Month = default(short),
        NumInvoices = default(int),
        TotalRate = default(decimal),
        TotalNumTreatments = default(double)
      });
      Assert.That(actual, Is.Not.Null);
      Assert.That(actual.Count(), Is.GreaterThan(0));
    }

    [Test]
    public void CanExecuteInsertQuery() {
      const string query = @"INSERT INTO Country (Code, CountryName) VALUES (@Code, @CountryName)";
      var actual = _executor.NewQuery(query)
                            .WithParameters(new {
                              Code = "AM",
                              CountryName = "Armenia"
                            })
                            .Execute();
      Assert.That(actual, Is.GreaterThan(0));
    }
  }
}