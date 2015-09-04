using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Security;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  [TestFixture]
  public class LogoutCommandHandlerTests {
    LogoutCommandHandler _sut;

    [SetUp]
    public void SetUp() {
      _sut = new LogoutCommandHandler();
    }

    [Test]
    public void ConstructorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }

    [Test]
    public void DelegatesControlToAuthenticatedUserApplyer() {
      var securityContext = A.Fake<ISecurityContext>();
      var command = new LogoutCommand {
        SecurityContext = securityContext
      };

      _sut.Handle(command).Wait();

      A.CallTo(() => securityContext.SetAuthenticatedUser(null))
       .MustHaveHappened();
    }
  }
}