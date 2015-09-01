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

    public class BasicCRUDTests : DapperTests {
      [Test]
      public void CanExecuteSelectQuery() {
        const string selectQuery = @"
          SELECT IL.Country AS CountryCode, C.CountryName, IL.Year, IL.Month, COUNT(IL.ID) AS NumInvoices, SUM(IL.TotalRate) AS TotalRate, SUM(IL.NumTreatments) AS TotalNumTreatments
          FROM q_InvoiceList AS IL INNER JOIN Country AS C ON C.Code = IL.Country
          GROUP BY IL.Country, C.CountryName, IL.Year, IL.Month;";
        var actualLoadedRecords = _executor.NewQuery(selectQuery).ExecuteWithAnonymousResult(new {
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

    public class ComplexQueryTests : DapperTests {
      [Test]
      public void CanExecuteComplexSelectQuery() {
        const string selectQuery =
          @"SELECT I.ID, P.ID AS PatientID, P.FirstName, P.LastName, P.FullName, P.BirthDate, P.Gender, P.[Address], P.PostalCode, P.City, P.Country, P.CountryName, P.CityDescription, PT.ID AS PerformedTreatmentID, PT.TreatmentDate, PT.ToothNumber, T.ID AS TreatmentID, T.Code, T.Description, TR.Rate, TT.CategoryID, TT.CategoryCode, TT.CategoryName, IIf(TT.CategoryCode Is Null,'',TT.CategoryCode & ' - ') & TT.CategoryName AS CategoryAndCode
            FROM ((((Invoice AS I INNER JOIN PatientTreatment AS PT ON PT.InvoiceID = I.ID) INNER JOIN iq_PatientHelper AS P ON (P.EndDate >= PT.TreatmentDate) AND (P.StartDate <= PT.TreatmentDate) AND (P.ID = I.PatientID)) INNER JOIN Treatment AS T ON T.ID = PT.TreatmentID) INNER JOIN q_TreatmentRate AS TR ON (TR.EndDate >= PT.TreatmentDate) AND (TR.StartDate <= PT.TreatmentDate) AND (TR.ID = T.ID)) INNER JOIN iq_TreatmentTree AS TT ON (TT.EndDate >= PT.TreatmentDate) AND (TT.StartDate <= PT.TreatmentDate) AND (TT.ID = T.ID);";

        var actualLoadedRecords = _executor.NewQuery(selectQuery).ExecuteWithAnonymousResult(new {
          ID = default(int),
          PatientID = default(int),
          FirstName = default(string),
          LastName = default(string),
          FullName = default(string),
          BirthDate = default(DateTime),
          Gender = default(string),
          Address = default(string),
          PostalCode = default(string),
          City = default(string),
          Country = default(string),
          CountryName = default(string),
          CityDescription = default(string),
          PerformedTreatmentID = default(int),
          TreatmentDate = default(DateTime),
          ToothNumber = default(byte),
          TreatmentID = default(int),
          Code = default(string),
          Description = default(string),
          Rate = default(decimal),
          CategoryID = default(int),
          CategoryCode = default(string),
          CategoryName = default(string),
          CategoryAndCode = default(string)
        });
        Assert.That(actualLoadedRecords, Is.Not.Null);
        Assert.That(actualLoadedRecords.Count(), Is.GreaterThan(0));
      }

      [Test]
      public void CanExecuteMultiJoinSelectQuery() {
        const string selectQuery = @"
SELECT
	I.ID,
	PT.ID AS PerformedTreatmentID,
	PT.TreatmentDate,
	PT.ToothNumber,
	T.ID AS TreatmentID,
	T.Code,
	T.[Description]
FROM 
  ((
    Invoice AS I INNER JOIN PatientTreatment AS PT ON PT.InvoiceID = I.ID) INNER JOIN
	    Treatment AS T ON T.ID = PT.TreatmentID)";

        var actualLoadedRecords = _executor.NewQuery(selectQuery).ExecuteWithAnonymousResult(new {
          ID = default(int),
          PerformedTreatmentID = default(int),
          TreatmentDate = default(DateTime),
          ToothNumber = default(byte),
          TreatmentID = default(int),
          Code = default(string),
          Description = default(string)
        });
        Assert.That(actualLoadedRecords, Is.Not.Null);
        Assert.That(actualLoadedRecords.Count(), Is.GreaterThan(0));
      }
    }
  }
}