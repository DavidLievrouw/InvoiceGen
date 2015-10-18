using Autofac;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.InvoiceGen.Security.Nancy;
using DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking;

namespace DavidLievrouw.InvoiceGen.Composition {
  public class SecurityModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      // Session hijacking
      builder.RegisterType<AntiSessionHijackLogic>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<SessionDetector>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<SessionHijackDetector>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<SessionAntiHijackHashInjector>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<SessionAntiHijackHashStripper>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<SessionAntiHijackHashGenerator>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<ResponseBuilderWhenSessionIsHijacked>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<SecureSessionCookieReader>()
             .AsImplementedInterfaces()
             .SingleInstance();

      // Logged in identity loading
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