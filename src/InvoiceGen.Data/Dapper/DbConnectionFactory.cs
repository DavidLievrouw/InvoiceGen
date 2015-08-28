using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace DavidLievrouw.InvoiceGen.Data.Dapper {
  public class DbConnectionFactory : IDbConnectionFactory {
    readonly ConnectionStringSettings _connectionStringSettings;
    readonly Lazy<DbProviderFactory> _dbProviderFactory;

    public DbConnectionFactory(ConnectionStringSettings connectionStringSettings) {
      if (connectionStringSettings == null) throw new ArgumentNullException("connectionStringSettings");
      _connectionStringSettings = connectionStringSettings;
      _dbProviderFactory = new Lazy<DbProviderFactory>(() => DbProviderFactories.GetFactory(_connectionStringSettings.ProviderName));
    }

    public IDbConnection OpenConnection() {
      var connection = _dbProviderFactory.Value.CreateConnection();
      Trace.Assert(connection != null, "connection != null");
      connection.ConnectionString = _connectionStringSettings.ConnectionString;
      connection.Open();
      return connection;
    }
  }
}