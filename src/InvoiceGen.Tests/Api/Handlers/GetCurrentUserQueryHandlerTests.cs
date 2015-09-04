using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.InvoiceGen.Security;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  [TestFixture]
  public class GetCurrentUserQueryHandlerTests {
    GetCurrentUserQueryHandler _sut;
    ISecurityContext _securityContext;

    [SetUp]
    public void SetUp() {
      _sut = new GetCurrentUserQueryHandler();
      _securityContext = _securityContext.Fake();
    }

    [Test]
    public void GivenContextWithoutUser_ReturnsNull() {
      ConfigureSecurityContext_ToReturn(null);
      var request = new GetCurrentUserRequest {
        SecurityContext = _securityContext
      };

      var actual = _sut.Handle(request).Result;

      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithValidUser_ReturnsUser() {
      var user = new User {Login = new Login {Value = "JohnD"}};
      ConfigureSecurityContext_ToReturn(user);
      var request = new GetCurrentUserRequest {
        SecurityContext = _securityContext
      };

      var actual = _sut.Handle(request).Result;

      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.EqualTo(user));
    }

    void ConfigureSecurityContext_ToReturn(User user) {
      A.CallTo(() => _securityContext.GetAuthenticatedUser())
       .Returns(user);
    }
  }
}