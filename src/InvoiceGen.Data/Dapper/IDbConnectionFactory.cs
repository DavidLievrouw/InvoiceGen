using System.Data;

namespace DavidLievrouw.InvoiceGen.Data.Dapper {
  public interface IDbConnectionFactory {
    IDbConnection OpenConnection();
  }
}