using System;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using DavidLievrouw.InvoiceGen.Domain;
using FakeItEasy;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class NancySecurityContextTests {
    ISessionFromContextResolver _sessionFromContextResolver;
    IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;
    NancyContext _nancyContext;
    NancySecurityContext _sut;

    [SetUp]
    public void SetUp() {
      _sessionFromContextResolver = _sessionFromContextResolver.Fake();
      _invoiceGenIdentityFactory = _invoiceGenIdentityFactory.Fake();
      _nancyContext = new NancyContext();
      _sut = new NancySecurityContext(_nancyContext, _sessionFromContextResolver, _invoiceGenIdentityFactory);
    }

    [Test]
    public void ConstructorTests() {
      Assert.Throws<ArgumentNullException>(() => new NancySecurityContext(null, _sessionFromContextResolver, _invoiceGenIdentityFactory));
      Assert.Throws<ArgumentNullException>(() => new NancySecurityContext(_nancyContext, null, _invoiceGenIdentityFactory));
      Assert.Throws<ArgumentNullException>(() => new NancySecurityContext(_nancyContext, _sessionFromContextResolver, null));
    }

    [Test]
    public void SetsUserInNancyContext() {
      var user = new User();
      var identity = new InvoiceGenIdentity(user);
      ConfigureInvoiceGenIdentityFactory_ToReturn(user, identity);

      _sut.AuthenticatedUser = user;
      var actual = _nancyContext.CurrentUser;

      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.EqualTo(identity));
    }

    [Test]
    public void GivenNullUser_SetsNullUserInNancyContext() {
      ConfigureInvoiceGenIdentityFactory_ToReturn(null, null);

      _sut.AuthenticatedUser = null;
      var actual = _nancyContext.CurrentUser;

      Assert.That(actual, Is.Null);
    }

    [Test]
    public void WhenNoIdentityIsSetInNancyContext_ReturnsNullUser() {
      _nancyContext.CurrentUser = null;
      var actual = _sut.AuthenticatedUser;
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void WhenInvalidIdentityIsSetInNancyContext_ReturnsNullUser() {
      _nancyContext.CurrentUser = new FakeUserIdentity("Pol");
      var actual = _sut.AuthenticatedUser;
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void WhenIdentityIsSetInNancyContext_ReturnsUserFromIdentity() {
      var user = new User();
      var identity = new InvoiceGenIdentity(user);
      _nancyContext.CurrentUser = identity;

      var actual = _sut.AuthenticatedUser;
      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.EqualTo(user));
    }

    [Test]
    public void ReturnsSessionFromResolver() {
      var session = A.Fake<ISession>();
      ConfigureSessionFromContextResolver_ToReturn(session);

      var actual = _sut.Session;

      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.EqualTo(session));
      A.CallTo(() => _sessionFromContextResolver.ResolveSession(_nancyContext))
       .MustHaveHappened();
    }

    void ConfigureSessionFromContextResolver_ToReturn(ISession session) {
      A.CallTo(() => _sessionFromContextResolver.ResolveSession(A<NancyContext>._))
       .Returns(session);
    }

    void ConfigureInvoiceGenIdentityFactory_ToReturn(User user, InvoiceGenIdentity identity) {
      A.CallTo(() => _invoiceGenIdentityFactory.Create(user))
       .Returns(identity);
    }
  }
}