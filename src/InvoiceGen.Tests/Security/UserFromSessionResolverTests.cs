using System;
using System.Collections.Generic;
using System.Web;
using DavidLievrouw.InvoiceGen.Domain;
using FakeItEasy;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class UserFromSessionResolverTests {
    const string RequestEnvironmentKey = "OWIN_REQUEST_ENVIRONMENT";
    NancyContext _nancyContext;
    IDictionary<string, object> _owinEnvironment;
    ISessionResolver _sessionResolver;
    UserFromSessionResolver _sut;

    [SetUp]
    public void SetUp() {
      _nancyContext = new NancyContext();
      _owinEnvironment = new Dictionary<string, object>();
      _nancyContext.Items.Add(RequestEnvironmentKey, _owinEnvironment);

      _sessionResolver = _sessionResolver.Fake();
      _sut = new UserFromSessionResolver(_sessionResolver);
    }

    [Test]
    public void ConstructorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }

    [Test]
    public void GivenNullContext_Throws() {
      Assert.Throws<ArgumentNullException>(() => _sut.ResolveUser(null));
    }

    [Test]
    public void GivenContextWithoutSession_ReturnsNull() {
      ConfigureSessionResolver_ToReturn(null);

      var actual = _sut.ResolveUser(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithSessionWithoutUser_ReturnsNull() {
      var session = new FakeSession();
      ConfigureSessionResolver_ToReturn(session);
      var actual = _sut.ResolveUser(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithSessionWithNullUser_ReturnsNull() {
      var session = new FakeSession();
      session["user"] = null;
      ConfigureSessionResolver_ToReturn(session);
      var actual = _sut.ResolveUser(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithSessionWithUser_ReturnsUser() {
      var user = new User();
      var session = new FakeSession();
      session["user"] = user;
      ConfigureSessionResolver_ToReturn(session);
      var actual = _sut.ResolveUser(_nancyContext);
      Assert.That(actual, Is.EqualTo(user));
    }

    void ConfigureSessionResolver_ToReturn(HttpSessionStateBase session) {
      A.CallTo(() => _sessionResolver.ResolveSession(A<NancyContext>._))
       .Returns(session);
    }

    class FakeSession : HttpSessionStateBase {
      readonly Dictionary<string, object> _items;

      public FakeSession() {
        _items = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
      }

      public override object this[string name] {
        get {
          return _items.ContainsKey(name)
            ? _items[name]
            : null;
        }
        set { _items[name] = value; }
      }
    }
  }
}