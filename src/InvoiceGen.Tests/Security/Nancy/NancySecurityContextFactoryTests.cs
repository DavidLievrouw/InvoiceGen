using System;
using DavidLievrouw.Utils.ForTesting.DotNet;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  [TestFixture]
  public class NancySecurityContextFactoryTests {
    INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;
    NancySecurityContextFactory _sut;

    [SetUp]
    public void SetUp() {
      _nancySessionFromNancyContextResolver = _nancySessionFromNancyContextResolver.Fake();
      _invoiceGenIdentityFactory = _invoiceGenIdentityFactory.Fake();
      _sut = new NancySecurityContextFactory(_nancySessionFromNancyContextResolver, _invoiceGenIdentityFactory);
    }

    [Test]
    public void ConstructorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }

    [Test]
    public void GivenNullNancyContext_Throws() {
      Assert.Throws<ArgumentNullException>(() => _sut.Create(null));
    }

    [Test]
    public void GivenValidNancyContext_CreatesNewInstance() {
      var nancyContext = new NancyContext();
      var actual = _sut.Create(nancyContext);
      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.InstanceOf<NancySecurityContext>());
    }
  }
}