using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Cookies;
using Nancy.Session;

namespace DavidLievrouw.InvoiceGen.Security {
  public class MemoryCacheBasedSessions {
    const string SessionIdKey = "_ncs";

    public static void Enable(IPipelines pipelines) {
      var store = new MemoryCacheBasedSessions(MemoryCache.Default);

      pipelines.BeforeRequest.AddItemToStartOfPipeline(ctx => LoadSession(ctx, store));
      pipelines.AfterRequest.AddItemToEndOfPipeline(ctx => SaveSession(ctx, store));
    }

    readonly ObjectCache _cache;

    public MemoryCacheBasedSessions(ObjectCache cache) {
      _cache = cache;
    }

    /// <summary>
    ///   Gets the cookie name in which the session Id is stored
    /// </summary>
    /// <value>Cookie name</value>
    public string CookieName => SessionIdKey;

    static int GetSessionTimeout() {
      return ConfigurationManager.AppSettings["Nancy:MemoryCacheBasedSessions:SessionTimeoutMinutes"] != null
        ? int.Parse(ConfigurationManager.AppSettings["Nancy:MemoryCacheBasedSessions:SessionTimeoutMinutes"])
        : 30;
    }

    /// <summary>
    ///   Save the session data to the memory cache, and set the session id cookie
    ///   in the response
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="session"></param>
    /// <param name="response"></param>
    public void Save(string sessionId, global::Nancy.Session.ISession session, Response response) {
      if (session == null) return;

      var dict = new Dictionary<string, object>();
      foreach (var kvp in session) {
        dict[kvp.Key] = kvp.Value;
      }

      var timeout = GetSessionTimeout();
      var cookie = new NancyCookie(SessionIdKey, sessionId) {
        Expires = DateTime.UtcNow.AddMinutes(timeout)
      };
      response.WithCookie(cookie);

      _cache.Set(sessionId, dict, DateTime.Now + TimeSpan.FromMinutes(timeout + 1));
    }

    /// <summary>
    ///   Load the session data from the nancy context
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public global::Nancy.Session.ISession Load(NancyContext context) {
      var request = context.Request;

      if (request.Cookies.ContainsKey(SessionIdKey) && _cache.Any(kvp => kvp.Key == request.Cookies[SessionIdKey])) {
        return new Session(_cache[request.Cookies[SessionIdKey]] as Dictionary<string, object>);
      }

      return new Session(new Dictionary<string, object>());
    }

    static void SaveSession(NancyContext context, MemoryCacheBasedSessions sessionStore) {
      var sessionId = context.Request.Cookies.ContainsKey(SessionIdKey)
        ? context.Request.Cookies[SessionIdKey]
        : Guid.NewGuid().ToString();

      sessionStore.Save(sessionId, context.Request.Session, context.Response);
    }

    /// <summary>
    ///   Loads the request session
    /// </summary>
    /// <param name="context">Nancy context</param>
    /// <param name="sessionStore">Session store</param>
    /// <returns>Always returns null</returns>
    static Response LoadSession(NancyContext context, MemoryCacheBasedSessions sessionStore) {
      context.Request.Session = sessionStore.Load(context);
      return null;
    }
  }
}