using System.Web.Configuration;
using Autofac;
using DavidLievrouw.InvoiceGen.Configuration;
using Newtonsoft.Json;

namespace DavidLievrouw.InvoiceGen.Composition {
  public static class CompositionRoot {
    public static IContainer Compose() {
      return Compose(WebConfigurationManager.OpenWebConfiguration("~/"));
    }

    public static IContainer Compose(System.Configuration.Configuration configuration) {
      var builder = new ContainerBuilder();

      builder.RegisterType<CustomJsonSerializer>()
             .As<JsonSerializer>()
             .AsImplementedInterfaces()
             .SingleInstance();

      builder.RegisterModule<SecurityModule>();
      builder.RegisterModule<NancyModule>();

      return builder.Build();
    }
  }
}