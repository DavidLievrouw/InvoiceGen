using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nancy;
using Nancy.Culture;
using Nancy.Testing;
using Nancy.ViewEngines;

namespace DavidLievrouw.InvoiceGen.Api {
  /// <summary>
  ///   Bootstrapper that specifies settings so that the unit tests run much faster.
  /// </summary>
  public class LightweightBootstrapper : ConfigurableBootstrapper {
    public LightweightBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration) : base(with => {
      with.DisableAutoApplicationStartupRegistration();
      with.DisableAutoRequestStartupRegistration();
      with.ViewLocationProvider<NoopViewLocatorProvider>();
      with.CultureService<NoopCultureService>();
      configuration(with);
    }) { }

    protected override byte[] FavIcon => null;

    class NoopViewLocatorProvider : IViewLocationProvider {
      public IEnumerable<ViewLocationResult> GetLocatedViews(IEnumerable<string> supportedViewExtensions) {
        return Enumerable.Empty<ViewLocationResult>();
      }

      public IEnumerable<ViewLocationResult> GetLocatedViews(IEnumerable<string> supportedViewExtensions, string location, string viewName) {
        return Enumerable.Empty<ViewLocationResult>();
      }
    }

    class NoopCultureService : ICultureService {
      public CultureInfo DetermineCurrentCulture(NancyContext context) {
        return System.Threading.Thread.CurrentThread.CurrentCulture;
      }
    }
  }
}