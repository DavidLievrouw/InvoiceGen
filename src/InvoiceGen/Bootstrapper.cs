using System;
using Autofac;
using DavidLievrouw.InvoiceGen.Security;
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
      StaticConfiguration.DisableErrorTraces = false;
      
      pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx => {
        var userResolver = container.Resolve<IUserFromSessionResolver>();
        var identityFactory = container.Resolve<IInvoiceGenIdentityFactory>();
        ctx.CurrentUser = identityFactory.Create(userResolver.ResolveUser(ctx));
        return null;
      });

      base.ApplicationStartup(container, pipelines);
    }
  }
}