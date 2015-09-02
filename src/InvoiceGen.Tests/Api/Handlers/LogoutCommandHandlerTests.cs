using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Security;
using FakeItEasy;
using Nancy;
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
      var nancyContext = new NancyContext();
      var authenticatedUserApplyer = A.Fake<IAuthenticatedUserApplyer>();
      ConfigureApplyerFactory_ToReturn(nancyContext, authenticatedUserApplyer);
      var command = new LogoutCommand {
        NancyContext = nancyContext
      };

      _sut.Handle(command).Wait();

      A.CallTo(() => _authenticatedUserApplyerFactory.Create(nancyContext))
       .MustHaveHappened();
      A.CallTo(() => authenticatedUserApplyer.ClearAuthenticatedUser())
       .MustHaveHappened();
    }

    void ConfigureApplyerFactory_ToReturn(NancyContext nancyContext, IAuthenticatedUserApplyer authenticatedUserApplyer) {
      A.CallTo(() => _authenticatedUserApplyerFactory.Create(nancyContext))
       .Returns(authenticatedUserApplyer);
    }
  }
}