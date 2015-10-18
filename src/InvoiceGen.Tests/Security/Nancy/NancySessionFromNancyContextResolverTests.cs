using System;
using Nancy;
using Nancy.Session;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  [TestFixture]
  public class NancySessionFromNancyContextResolverTests {
    NancyContext _nancyContext;
    NancySessionFromNancyContextResolver _sut;

    [SetUp]
    public void SetUp() {
      _nancyContext = new NancyContext {
        Request = new Request("GET", "/", "http") {
          Session = new Session()
        }
      };

      _sut = new NancySessionFromNancyContextResolver();
    }

    [Test]
    public void GivenNullContext_Throws() {
      Assert.Throws<ArgumentNullException>(() => _sut.ResolveSession(null));
    }

    [Test]
    public void GivenContextContainsNoSession_ReturnsNull() {
      _nancyContext.Request.Session = null;

      var actual = _sut.ResolveSession(_nancyContext);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithSession_ReturnsSession() {
      var actual = _sut.ResolveSession(_nancyContext);
      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.InstanceOf<NancySession>());
    }
  }
}