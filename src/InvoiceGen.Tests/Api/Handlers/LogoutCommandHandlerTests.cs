using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Security;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  [TestFixture]
  public class LogoutCommandHandlerTests {
    IAuthenticatedUserApplyerFactory _authenticatedUserApplyerFactory;
    LogoutCommandHandler _sut;

    [SetUp]
    public void SetUp() {
      _authenticatedUserApplyerFactory = _authenticatedUserApplyerFactory.Fake();
      _sut = new LogoutCommandHandler(_authenticatedUserApplyerFactory);
    }

    [Test]
    public void ConstructorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }

    [Test]
    public void DelegatesControlToAuthenticatedUserApplyer() {
      var securityContext = A.Fake<ISecurityContext>();
      var authenticatedUserApplyer = A.Fake<IAuthenticatedUserApplyer>();
      ConfigureApplyerFactory_ToReturn(securityContext, authenticatedUserApplyer);
      var command = new LogoutCommand {
        SecurityContext = securityContext
      };

      _sut.Handle(command).Wait();

      A.CallTo(() => _authenticatedUserApplyerFactory.Create(securityContext))
       .MustHaveHappened();
      A.CallTo(() => authenticatedUserApplyer.ClearAuthenticatedUser())
       .MustHaveHappened();
    }

    void ConfigureApplyerFactory_ToReturn(ISecurityContext securityContext, IAuthenticatedUserApplyer authenticatedUserApplyer) {
      A.CallTo(() => _authenticatedUserApplyerFactory.Create(securityContext))
       .Returns(authenticatedUserApplyer);
    }
  }
}