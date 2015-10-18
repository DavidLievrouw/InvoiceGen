using System;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class AntiSessionHijackLogic : IAntiSessionHijackLogic {
    readonly IResponseBuilderWhenSessionIsHijacked _responseBuilderWhenSessionIsHijacked;
    readonly ISessionHijackDetector _sessionHijackDetector;
    readonly ISessionAntiHijackHashStripper _sessionAntiHijackHashStripper;
    readonly ISessionAntiHijackHashInjector _sessionAntiHijackHashInjector;

    public AntiSessionHijackLogic(
      IResponseBuilderWhenSessionIsHijacked responseBuilderWhenSessionIsHijacked,
      ISessionHijackDetector sessionHijackDetector,
      ISessionAntiHijackHashStripper sessionAntiHijackHashStripper,
      ISessionAntiHijackHashInjector sessionAntiHijackHashInjector) {
      if (responseBuilderWhenSessionIsHijacked == null) throw new ArgumentNullException(nameof(responseBuilderWhenSessionIsHijacked));
      if (sessionHijackDetector == null) throw new ArgumentNullException(nameof(sessionHijackDetector));
      if (sessionAntiHijackHashStripper == null) throw new ArgumentNullException(nameof(sessionAntiHijackHashStripper));
      if (sessionAntiHijackHashInjector == null) throw new ArgumentNullException(nameof(sessionAntiHijackHashInjector));
      _responseBuilderWhenSessionIsHijacked = responseBuilderWhenSessionIsHijacked;
      _sessionHijackDetector = sessionHijackDetector;
      _sessionAntiHijackHashStripper = sessionAntiHijackHashStripper;
      _sessionAntiHijackHashInjector = sessionAntiHijackHashInjector;
    }

    public Response InterceptHijackedSession(Request request) {
      var sessionIsHijacked = _sessionHijackDetector.IsSessionHijacked(request);
      _sessionAntiHijackHashStripper.StripHashFromCookie(request);
      return sessionIsHijacked ? _responseBuilderWhenSessionIsHijacked.BuildHijackedResponse() : null;
    }

    public void ProtectResponseFromSessionHijacking(NancyContext context) {
      _sessionAntiHijackHashInjector.InjectHashInCookie(context);
    }
  }
}