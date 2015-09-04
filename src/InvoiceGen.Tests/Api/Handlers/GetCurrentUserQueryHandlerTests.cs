using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  [TestFixture]
  public class GetCurrentUserQueryHandlerTests {
    GetCurrentUserQueryHandler _sut;

    [SetUp]
    public void SetUp() {
      _sut = new GetCurrentUserQueryHandler();
    }

    [Test]
    public void GivenContextWithoutUser_ReturnsNull() {
      var request = new GetCurrentUserRequest {
        SecurityContext = new FakeSecurityContext {
          AuthenticatedUser = null
        }
      };

      var actual = _sut.Handle(request).Result;

      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithValidUser_ReturnsUser() {
      var user = new User {Login = new Login {Value = "JohnD"}};
      var request = new GetCurrentUserRequest {
        SecurityContext = new FakeSecurityContext {
          AuthenticatedUser = user
        }
      };

      var actual = _sut.Handle(request).Result;

      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.EqualTo(user));
    }
  }
}