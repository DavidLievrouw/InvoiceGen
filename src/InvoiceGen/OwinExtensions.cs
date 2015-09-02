using System;
using System.Web;
using System.Web.SessionState;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Nancy;
using Nancy.Owin;
using Owin;

namespace DavidLievrouw.InvoiceGen {
  public static class OwinExtensions {
    public static IAppBuilder RequireAspNetSession(this IAppBuilder app) {
      app.Use((context, next) => {
        context.Get<HttpContextBase>().SetSessionStateBehavior(SessionStateBehavior.Required);
        return next();
      });
      app.UseStageMarker(PipelineStage.MapHandler);

      return app;
    }

    public static T Get<T>(this IOwinContext context) {
      return context.Get<T>(typeof(T).FullName);
    }

    public static HttpContextBase GetHttpContext(this NancyContext context) {
      if (context == null) throw new ArgumentNullException("context");

      var owinEnvironment = context.GetOwinEnvironment();
      var httpContextKey = typeof(HttpContextBase).FullName;

      object httpContextObject;
      if (owinEnvironment != null && owinEnvironment.TryGetValue(httpContextKey, out httpContextObject)) {
        return httpContextObject as HttpContextBase;
      }

      return null;
    }
  }
}