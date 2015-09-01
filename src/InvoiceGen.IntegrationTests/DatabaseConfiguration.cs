using System;
using System.Configuration;
using System.Data.OleDb;
using System.IO;

namespace DavidLievrouw.InvoiceGen {
  public class DatabaseConfiguration {
    public DatabaseConfiguration(ConnectionStringSettings connectionStringSettings) {
      if (connectionStringSettings == null) throw new ArgumentNullException("connectionStringSettings");
      ConnectionStringSettings = connectionStringSettings;
    }

    public ConnectionStringSettings ConnectionStringSettings { get; private set; }

    public FileInfo DatabaseFile {
      get {
        var builder = new OleDbConnectionStringBuilder(ConnectionStringSettings.ConnectionString);
        return new FileInfo(builder.DataSource);
      }
    }
  }
}