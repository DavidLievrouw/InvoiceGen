using System;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class AuthenticatedUserApplyerFactoryTests {
    AuthenticatedUserApplyerFactory _sut;

    [SetUp]
    public void SetUp() {
      _sut = new AuthenticatedUserApplyerFactory();
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
      var securityContext = A.Fake<ISecurityContext>();
      var actual = _sut.Create(securityContext);
      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.InstanceOf<AuthenticatedUserApplyer>());
    }
  }
}