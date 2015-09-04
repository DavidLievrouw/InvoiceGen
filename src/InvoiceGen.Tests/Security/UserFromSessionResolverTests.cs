using DavidLievrouw.InvoiceGen.Domain.DTO;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class UserFromSessionResolverTests {
    ISession _session;
    UserFromSessionResolver _sut;

    [SetUp]
    public void SetUp() {
      _session = new FakeSession();
      _sut = new UserFromSessionResolver();
    }

    [Test]
    public void ConstructorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }

    [Test]
    public void GivenNullSession_ReturnsNull() {
      var actual = _sut.ResolveUser(null);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithSessionWithoutUser_ReturnsNull() {
      var actual = _sut.ResolveUser(_session);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithSessionWithNullUser_ReturnsNull() {
      _session[Constants.SessionKeyForUser] = null;
      var actual = _sut.ResolveUser(_session);
      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithSessionWithUser_ReturnsUser() {
      var user = new User();
      _session[Constants.SessionKeyForUser] = user;
      var actual = _sut.ResolveUser(_session);
      Assert.That(actual, Is.EqualTo(user));
    }
  }
}