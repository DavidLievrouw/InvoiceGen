using System;
using DavidLievrouw.InvoiceGen.Configuration;
using Nancy.Testing;

namespace DavidLievrouw.InvoiceGen {
  public class AppBootstrapper : ConfigurableBootstrapper {
    public AppBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration) : base(configuration) {
      InternalConfiguration.Serializers.Clear();
      InternalConfiguration.Serializers.Add(typeof(CustomJsonSerializer));
    }
  }
}