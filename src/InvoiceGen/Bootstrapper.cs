using System;
using Autofac;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.InvoiceGen.Security.Nancy;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;

namespace DavidLievrouw.InvoiceGen {
  public class Bootstrapper : AutofacNancyBootstrapper {
    readonly IContainer _container;

    public Bootstrapper(IContainer container) {
      if (container == null) throw new ArgumentNullException("container");
      _container = container;
    }

    protected override ILifetimeScope GetApplicationContainer() {
      return _container;
    }

    protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines) {
      StaticConfiguration.DisableErrorTraces = false;

      // Enable memory sessions, and secure them against session hijacking
      MemoryCacheBasedSessions.Enable(pipelines);

      // Load the user from the AspNet session. If one is found, create a Nancy identity and assign it.
      pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx => {
        var identityAssigner = container.Resolve<INancyIdentityFromContextAssigner>();
        identityAssigner.AssignNancyIdentityFromContext(ctx);
        return null;
      });

      base.ApplicationStartup(container, pipelines);
    }
  }
}