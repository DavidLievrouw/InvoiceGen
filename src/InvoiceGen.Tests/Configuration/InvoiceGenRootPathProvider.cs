using System.IO;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Configuration {
  public class InvoiceGenRootPathProvider : IRootPathProvider {
    readonly string _rootPath;

    public InvoiceGenRootPathProvider() {
      var directoryName = Path.GetDirectoryName(typeof(Startup).Assembly.CodeBase);

      if (directoryName != null) {
        var assemblyPath = directoryName.Replace(@"file:\", string.Empty);
        _rootPath = Path.Combine(assemblyPath, "..", "..", "..", "InvoiceGen");
      }
    }

    public string GetRootPath() {
      return _rootPath;
    }
  }
}