using System;
using System.Security;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;

namespace DavidLievrouw.InvoiceGen {
  public static class ErrorPipelines {
    public static ErrorPipeline HandleModelBindingException() {
      return new Func<NancyContext, Exception, object>((context, ex) => {
        if (!(ex is ModelBindingException)) return null;
        return new Negotiator(context)
          .WithStatusCode(HttpStatusCode.BadRequest)
          .WithReasonPhrase(ex.Message)
          .WithContentType("application/json")
          .WithModel(ex.Message);
      });
    }

    public static ErrorPipeline HandleRequestValidationException() {
      return new Func<NancyContext, Exception, object>((context, ex) => {
        if (!(ex is RequestValidationException)) return null;
        return new Negotiator(context)
         .WithStatusCode(HttpStatusCode.BadRequest)
         .WithReasonPhrase(ex.Message)
         .WithContentType("application/json")
         .WithModel(ex.Message);
      });
    }

    public static ErrorPipeline HandleSecurityException() {
      return new Func<NancyContext, Exception, object>((context, ex) => {
        if (!(ex is SecurityException)) return null;
        return new Negotiator(context)
          .WithStatusCode(HttpStatusCode.Forbidden)
          .WithReasonPhrase(ex.Message)
          .WithContentType("application/json")
          .WithModel(ex.Message);
      });
    }
  }
}