using System.Linq;
using Autofac;
using Autofac.Core;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using DavidLievrouw.InvoiceGen.App;
using DavidLievrouw.InvoiceGen.Common;
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

      // register all command handlers
      builder.RegisterAssemblyTypes(nancyAssembly)
             .Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)))
             .As(t => new KeyedService("commandHandler", t.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>))));
      builder.RegisterGenericDecorator(typeof(ValidationAwareCommandHandler<>), typeof(ICommandHandler<>), fromKey: "commandHandler");

      // wrap all command handlers in INancyCommandHandler
      nancyAssembly.GetTypes()
                   .Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)))
                   .ForEach(handlerType => {
                     var implType = handlerType.GetInterfaces().Single(itf => itf.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
                     var genArg = implType.GetGenericArguments()[0];
                     builder.RegisterType(typeof(NancyCommandHandler<>).MakeGenericType(genArg))
                            .AsImplementedInterfaces();
                   });

      // register all query handlers
      builder.RegisterAssemblyTypes(nancyAssembly)
             .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<>)))
             .AsImplementedInterfaces();
      builder.RegisterAssemblyTypes(nancyAssembly)
             .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<,>)))
             .As(t => new KeyedService("queryHandler", t.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))));
      builder.RegisterGenericDecorator(typeof(ValidationAwareQueryHandler<,>), typeof(IQueryHandler<,>), fromKey: "queryHandler");

      // wrap all query handlers in INancyQueryHandler
      nancyAssembly.GetTypes()
                   .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<>)))
                   .ForEach(handlerType => {
                     var implType = handlerType.GetInterfaces().Single(itf => itf.GetGenericTypeDefinition() == typeof(IQueryHandler<>));
                     var genArg = implType.GetGenericArguments()[0];
                     builder.RegisterType(typeof(NancyQueryHandler<>).MakeGenericType(genArg))
                            .AsImplementedInterfaces();
                   });
      nancyAssembly.GetTypes()
                   .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<,>)))
                   .ForEach(handlerType => {
                     var implType = handlerType.GetInterfaces().Single(itf => itf.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
                     var genArg1 = implType.GetGenericArguments()[0];
                     var genArg2 = implType.GetGenericArguments()[1];
                     builder.RegisterType(typeof(NancyQueryHandler<,>).MakeGenericType(genArg1, genArg2))
                            .AsImplementedInterfaces();
                   });
    }
  }
}