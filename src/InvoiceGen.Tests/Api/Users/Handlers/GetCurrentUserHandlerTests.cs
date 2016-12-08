using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Users.Handlers {
  [TestFixture]
  public class GetCurrentUserHandlerTests {
    GetCurrentUserHandler _sut;
    ISecurityContext _securityContext;

    [SetUp]
    public void SetUp() {
      _sut = new GetCurrentUserHandler();
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