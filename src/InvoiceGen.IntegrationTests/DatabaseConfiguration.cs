using System;
using System.Configuration;

namespace DavidLievrouw.InvoiceGen {
  public class DatabaseConfiguration {
    public DatabaseConfiguration(ConnectionStringSettings connectionStringSettings) {
      if (connectionStringSettings == null) throw new ArgumentNullException(nameof(connectionStringSettings));
      ConnectionStringSettings = connectionStringSettings;
    }

    public ConnectionStringSettings ConnectionStringSettings { get; }
  }
}