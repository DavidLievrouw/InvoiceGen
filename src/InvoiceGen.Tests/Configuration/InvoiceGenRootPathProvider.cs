using System;
using System.IO;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Configuration {
  public class InvoiceGenRootPathProvider : IRootPathProvider {
    readonly string _rootPath;

    public InvoiceGenRootPathProvider() {
      var directoryName = Path.GetDirectoryName(typeof(Startup).Assembly.CodeBase);

      if (directoryName != null) {
        var subDirs = Path.Combine("..", "..", "..");
        if (Isx86Process()) subDirs = Path.Combine(subDirs, "..");
        var assemblyPath = directoryName.Replace(@"file:\", string.Empty);
        _rootPath = Path.Combine(assemblyPath, subDirs, "InvoiceGen");
      }
    }

    public string GetRootPath() {
      return _rootPath;
    }

    static bool Isx86Process() {
      return IntPtr.Size == 4;
    }
  }
}