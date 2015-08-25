using System;
using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;

namespace DavidLievrouw.InvoiceGen {
  public class Bootstrapper : AutofacNancyBootstrapper {
    readonly IContainer _container;

    public Bootstrapper(IContainer container) {
      if (container == null) throw new ArgumentNullException(nameof(container));
      _container = container;
    }

    protected override ILifetimeScope GetApplicationContainer() {
      return _container;
    }

    protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines) {
      base.ApplicationStartup(container, pipelines);
      StaticConfiguration.DisableErrorTraces = false;
    }
  }
}