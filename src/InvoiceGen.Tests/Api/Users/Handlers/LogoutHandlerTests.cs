using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils.ForTesting.DotNet;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Users.Handlers {
  [TestFixture]
  public class LogoutHandlerTests {
    LogoutHandler _sut;

    [SetUp]
    public void SetUp() {
      _sut = new LogoutHandler();
    }

    [Test]
    public void ConstructorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }

    [Test]
    public void DelegatesControlToAuthenticatedUserApplyer() {
      var securityContext = A.Fake<ISecurityContext>();
      var command = new LogoutRequest {
        SecurityContext = securityContext
      };

      _sut.Handle(command).Wait();

      A.CallTo(() => securityContext.SetAuthenticatedUser(null))
       .MustHaveHappened();
    }
  }
}