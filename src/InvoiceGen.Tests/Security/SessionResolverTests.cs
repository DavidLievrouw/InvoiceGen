using System;
using System.Collections.Generic;
using System.Web;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class SessionResolverTests {
    const string RequestEnvironmentKey = "OWIN_REQUEST_ENVIRONMENT";
    NancyContext _nancyContext;
    IDictionary<string, object> _owinEnvironment;
    SessionResolver _sut;

    [SetUp]
    public void SetUp() {
      _nancyContext = new NancyContext();
      _owinEnvironment = new Dictionary<string, object>();
      _nancyContext.Items.Add(RequestEnvironmentKey, _owinEnvironment);

      _sut = new SessionResolver();
    }

    [Test]
    public void GivenNullContext_Throws() {
      Assert.Throws<ArgumentNullException>(() => _sut.ResolveSession(null));
    }

    [Test]
    public void GivenContextContainsNoOwinEnvironment_ReturnsNull() {
      _nancyContext.Items.Clear();

      var actual = _sut.ResolveSession(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextContainsNoHttpContext_ReturnsNull() {
      var actual = _sut.ResolveSession(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextContainsNoSession_ReturnsNull() {
      var httpContext = new HttpContextBuilder()
        .New()
        .Build();
      _owinEnvironment.Add(typeof(HttpContextBase).FullName, httpContext);

      var actual = _sut.ResolveSession(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithSession_ReturnsSession() {
      var httpContext = new HttpContextBuilder()
        .New()
        .WithSession()
        .Build();
      _owinEnvironment.Add(typeof(HttpContextBase).FullName, httpContext);

      var actual = _sut.ResolveSession(_nancyContext);
      Assert.That(actual, Is.Not.Null);
    }
  }
}