using System;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class AuthenticatedUserApplyerFactoryTests {
    ISessionFromContextResolver _sessionFromContextResolver;
    IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;
    AuthenticatedUserApplyerFactory _sut;

    [SetUp]
    public void SetUp() {
      _sessionFromContextResolver = _sessionFromContextResolver.Fake();
      _invoiceGenIdentityFactory = _invoiceGenIdentityFactory.Fake();
      _sut = new AuthenticatedUserApplyerFactory(_sessionFromContextResolver, _invoiceGenIdentityFactory);
    }

    [Test]
    public void ConstructorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }

    [Test]
    public void GivenNullContext_Throws() {
      Assert.Throws<ArgumentNullException>(() => _sut.Create(null));
    }

    [Test]
    public void GivenValidContext_CreatesNewInstance() {
      var nancyContext = new NancyContext();
      var actual = _sut.Create(nancyContext);
      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.InstanceOf<AuthenticatedUserApplyer>());
    }
  }
}