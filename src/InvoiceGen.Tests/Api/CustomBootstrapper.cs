using System;
using DavidLievrouw.InvoiceGen.Configuration;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.InvoiceGen.Security.Nancy;
using Nancy.Testing;

namespace DavidLievrouw.InvoiceGen.Api {
  public class CustomBootstrapper : ConfigurableBootstrapper {
    public CustomBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration) : base(configuration) {
      InternalConfiguration.Serializers.Clear();
      InternalConfiguration.Serializers.Add(typeof(CustomJsonSerializer));
      BeforeRequest.AddItemToEndOfPipeline(context => {
        if (AuthenticatedUser == null) context.CurrentUser = null;
        else {
          var userIdentity = new InvoiceGenIdentity(AuthenticatedUser);
          context.CurrentUser = userIdentity;
        }
        return null;
      });
    }

    public User AuthenticatedUser { get; set; }
  }
}