using Autofac;
using DavidLievrouw.InvoiceGen.Security;

namespace DavidLievrouw.InvoiceGen.Composition {
  public class SecurityModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      builder.RegisterType<AspNetSessionFromContextResolver>()
             .AsImplementedInterfaces()
             .SingleInstance();

      builder.RegisterType<UserFromSessionResolver>()
             .AsImplementedInterfaces()
             .SingleInstance();

      builder.RegisterType<InvoiceGenIdentityFactory>()
             .AsImplementedInterfaces()
             .SingleInstance();

      builder.RegisterType<AuthenticatedUserApplyerFactory>()
             .AsImplementedInterfaces()
             .SingleInstance();
    }
  }
}