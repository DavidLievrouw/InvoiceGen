using Autofac;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.InvoiceGen.Security.Nancy;

namespace DavidLievrouw.InvoiceGen.Composition {
  public class SecurityModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      builder.RegisterType<NancySessionFromNancyContextResolver>()
             .AsImplementedInterfaces()
             .SingleInstance();

      builder.RegisterType<UserFromSessionResolver>()
             .AsImplementedInterfaces()
             .SingleInstance();

      builder.RegisterType<InvoiceGenIdentityFactory>()
             .AsImplementedInterfaces()
             .SingleInstance();
    }
  }
}