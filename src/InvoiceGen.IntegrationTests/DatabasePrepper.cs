using System;
using System.Configuration;
using System.Data.OleDb;
using System.IO;

namespace DavidLievrouw.InvoiceGen {
  public class DatabasePrepper {
    readonly DatabaseConfiguration _dbConfig;

    public DatabasePrepper(DatabaseConfiguration dbConfig) {
      if (dbConfig == null) throw new ArgumentNullException("dbConfig");
      _dbConfig = dbConfig;
    }

    public DatabaseConfiguration PrepareDatabaseForCurrentTest() {
      var uniqueId = Guid.NewGuid();
      var builder = new OleDbConnectionStringBuilder(_dbConfig.ConnectionStringSettings.ConnectionString);
      var originalDbFile = new FileInfo(builder.DataSource);
      var newFileFullName = Path.Combine(originalDbFile.Directory.FullName,
                                         string.Format("{0}_{1}{2}", 
                                          Path.GetFileNameWithoutExtension(originalDbFile.FullName), 
                                          uniqueId,
                                          Path.GetExtension(originalDbFile.FullName)));

      originalDbFile.CopyTo(newFileFullName);

      builder.DataSource = newFileFullName;
      return new DatabaseConfiguration(new ConnectionStringSettings {
        Name = _dbConfig.ConnectionStringSettings.Name,
        ProviderName = _dbConfig.ConnectionStringSettings.ProviderName,
        ConnectionString = builder.ConnectionString
      });
    }
  }
}