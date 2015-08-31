using System;
using System.Configuration;
using System.IO;

namespace DavidLievrouw.InvoiceGen {
  public static class DatabaseConfigurationReader {
    public static DatabaseConfiguration Read(System.Configuration.Configuration config) {
      if (config == null) throw new ArgumentNullException("config");

      var connectionStringSettings = config.ConnectionStrings.ConnectionStrings["AccessDB"];
      if (connectionStringSettings == null) throw new SettingsPropertyNotFoundException("The connection string was not found in the settings.");
      return new DatabaseConfiguration(connectionStringSettings);
    }

    public static DatabaseConfiguration Read() {
      var configMap = new ExeConfigurationFileMap {
        ExeConfigFilename = Path.GetFileName(typeof(DatabaseConfiguration).Assembly.Location) + ".config"
      };
      var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
      return Read(config);
    }
  }
}