using System.Linq;
using Autofac;
using Autofac.Core;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using DavidLievrouw.InvoiceGen.App;
using DavidLievrouw.InvoiceGen.Security.Nancy;
using DavidLievrouw.Utils;
using FluentValidation;

namespace DavidLievrouw.InvoiceGen.Composition {
  public class NancyModule : Module {
    protected override void Load(ContainerBuilder builder) {
      base.Load(builder);

      var nancyAssembly = typeof(AppModule).Assembly;

      // Register validators
      builder.RegisterAssemblyTypes(nancyAssembly)
             .AsClosedTypesOf(typeof(IValidator<>))
             .SingleInstance();

      // register all query handlers
      builder.RegisterAssemblyTypes(nancyAssembly)
             .Where(t => t.IsClosedTypeOf(typeof(IHandler<>)))
             .AsImplementedInterfaces();
      builder.RegisterAssemblyTypes(nancyAssembly)
             .Where(t => t.IsClosedTypeOf(typeof(IHandler<,>)))
             .As(t => new KeyedService("queryHandler", t.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandler<,>))));
      builder.RegisterGenericDecorator(typeof(RequestValidationAwareHandler<,>), typeof(IHandler<,>), "queryHandler");

      // Register other stuff
      builder.RegisterType<NancyIdentityFromContextAssigner>()
             .AsImplementedInterfaces()
             .SingleInstance();
      builder.RegisterType<NancySecurityContextFactory>()
             .AsImplementedInterfaces()
             .SingleInstance();
    }
  }
}