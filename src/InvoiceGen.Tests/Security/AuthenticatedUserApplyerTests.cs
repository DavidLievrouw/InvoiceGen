using System;
using DavidLievrouw.InvoiceGen.Domain;
using FakeItEasy;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class AuthenticatedUserApplyerTests {
    NancyContext _nancyContext;
    ISessionFromContextResolver _sessionFromContextResolver;
    IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;
    AuthenticatedUserApplyer _sut;

    [SetUp]
    public virtual void SetUp() {
      _sessionFromContextResolver = _sessionFromContextResolver.Fake();
      _invoiceGenIdentityFactory = _invoiceGenIdentityFactory.Fake();
      _nancyContext = new NancyContext();

      _sut = new AuthenticatedUserApplyer(_nancyContext, _sessionFromContextResolver, _invoiceGenIdentityFactory);
    }

    [Test]
    public void ConstructorTests() {
      Assert.Throws<ArgumentNullException>(() => new AuthenticatedUserApplyer(null, _sessionFromContextResolver, _invoiceGenIdentityFactory));
      Assert.Throws<ArgumentNullException>(() => new AuthenticatedUserApplyer(_nancyContext, null, _invoiceGenIdentityFactory));
      Assert.Throws<ArgumentNullException>(() => new AuthenticatedUserApplyer(_nancyContext, _sessionFromContextResolver, null));
    }

    public class ApplyAuthenticatedUser : AuthenticatedUserApplyerTests {
      User _user;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _user = new User();
      }

      [Test]
      public void GivenNullUser_Throws() {
        Assert.Throws<ArgumentNullException>(() => _sut.ApplyAuthenticatedUser(null));
      }

      [Test]
      public void WhenSessionDoesNotExist_Throws() {
        ConfigureSessionFromContextResolver_ToReturn(null);
        Assert.Throws<InvalidOperationException>(() => _sut.ApplyAuthenticatedUser(_user));
      }

      [Test]
      public void SetsUserInSession() {
        var session = new FakeSession();
        ConfigureSessionFromContextResolver_ToReturn(session);

        _sut.ApplyAuthenticatedUser(_user);

        Assert.That(session["user"], Is.Not.Null);
        Assert.That(session["user"], Is.EqualTo(_user));
      }

      [Test]
      public void SetsCreatedNancyIdentity() {
        var session = new FakeSession();
        var identity = new InvoiceGenIdentity(_user);
        ConfigureSessionFromContextResolver_ToReturn(session);
        ConfigureInvoiceGenIdentityFactory_ToReturn(_user, identity);

        _sut.ApplyAuthenticatedUser(_user);

        Assert.That(_nancyContext.CurrentUser, Is.Not.Null);
        Assert.That(_nancyContext.CurrentUser, Is.EqualTo(identity));
      }
    }

    public class ClearAuthenticatedUser : AuthenticatedUserApplyerTests {
      [Test]
      public void WhenSessionDoesNotExist_DoesNotThrow() {
        ConfigureSessionFromContextResolver_ToReturn(null);
        Assert.DoesNotThrow(() => _sut.ClearAuthenticatedUser());
      }

      [Test]
      public void WhenSessionExists_AbandonsSession_AndClearsData() {
        var session = new FakeSession();
        session["user"] = new User();
        ConfigureSessionFromContextResolver_ToReturn(session);

        _sut.ClearAuthenticatedUser();

        Assert.That(session.IsAbandoned);
        Assert.That(session["user"], Is.Null);
      }

      [Test]
      public void ClearsNancyIdentity() {
        _nancyContext.CurrentUser = new InvoiceGenIdentity(new User());
        _sut.ClearAuthenticatedUser();
        Assert.That(_nancyContext.CurrentUser, Is.Null);
      }
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