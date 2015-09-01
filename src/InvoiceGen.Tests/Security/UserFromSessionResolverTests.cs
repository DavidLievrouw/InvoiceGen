using System;
using System.Collections.Generic;
using System.Web;
using InvoiceGen.Domain;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class UserFromSessionResolverTests {
    const string RequestEnvironmentKey = "OWIN_REQUEST_ENVIRONMENT";
    NancyContext _nancyContext;
    IDictionary<string, object> _owinEnvironment;
    UserFromSessionResolver _sut;

    [SetUp]
    public void SetUp() {
      _nancyContext = new NancyContext();
      _owinEnvironment = new Dictionary<string, object>();
      _nancyContext.Items.Add(RequestEnvironmentKey, _owinEnvironment);

      _sut = new UserFromSessionResolver();
    }

    [Test]
    public void GivenNullContext_Throws() {
      Assert.Throws<ArgumentNullException>(() => _sut.ResolveUser(null));
    }

    [Test]
    public void GivenContextContainsNoOwinEnvironment_ReturnsNull() {
      _nancyContext.Items.Clear();

      var actual = _sut.ResolveUser(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextContainsNoHttpContext_ReturnsNull() {
      var actual = _sut.ResolveUser(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenHttpContextContainsNoSession_ReturnsNull() {
      var httpContext = new HttpContextBuilder()
        .New()
        .Build();
      _owinEnvironment.Add(typeof(HttpContextBase).FullName, httpContext);

      var actual = _sut.ResolveUser(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenSessionNoUser_ReturnsNull() {
      var httpContext = new HttpContextBuilder()
        .New()
        .WithUser(null)
        .Build();
      _owinEnvironment.Add(typeof(HttpContextBase).FullName, httpContext);

      var actual = _sut.ResolveUser(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenSessionWithUser_ReturnsUser() {
      var user = new User();
      var httpContext = new HttpContextBuilder()
        .New()
        .WithUser(user)
        .Build();
      _owinEnvironment.Add(typeof(HttpContextBase).FullName, httpContext);

      var actual = _sut.ResolveUser(_nancyContext);
      Assert.That(actual, Is.EqualTo(user));
    }
  }
}