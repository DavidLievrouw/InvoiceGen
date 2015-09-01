using DavidLievrouw.InvoiceGen.Domain;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class InvoiceGenIdentityFactoryTests {
    InvoiceGenIdentityFactory _sut;

    [SetUp]
    public void SetUp() {
      _sut = new InvoiceGenIdentityFactory();
    }

    [Test]
    public void GivenNullUser_ReturnsNull() {
      var actual = _sut.Create(null);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenValidUser_CreatesIdentityByUser() {
      var user = new User();
      var actual = _sut.Create(user);

      Assert.That(actual, Is.Not.Null);
      Assert.That(actual.User, Is.EqualTo(user));
    }
  }
}