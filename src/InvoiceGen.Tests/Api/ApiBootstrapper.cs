using System;
using DavidLievrouw.InvoiceGen.Configuration;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.InvoiceGen.Security.Nancy;

namespace DavidLievrouw.InvoiceGen.Api {
  public class ApiBootstrapper : LightweightBootstrapper {
    public ApiBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration) : base(configuration) {
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

      OnError = OnError
        + ErrorPipelines.HandleModelBindingException()
        + ErrorPipelines.HandleRequestValidationException()
        + ErrorPipelines.HandleSecurityException();
    }

    public User AuthenticatedUser { get; set; }
  }
}