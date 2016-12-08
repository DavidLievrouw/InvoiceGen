using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Configuration;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.InvoiceGen.Security.Nancy;
using DavidLievrouw.Utils;
using FakeItEasy;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api {
  [TestFixture]
  public partial class UsersModuleTests {
    CustomBootstrapper _bootstrapper;
    IHandler<GetCurrentUserRequest, User> _getCurrentUserQueryHandler;
    IHandler<LoginCommand, bool> _loginCommandHandler;
    IHandler<LogoutCommand, bool> _logoutCommandHandler;
    FakeNancyHandler<GetCurrentUserRequest, User> _getCurrentUserNancyHandler;
    FakeNancyHandler<LoginCommand, bool> _loginNancyCommandHandler;
    FakeNancyHandler<LogoutCommand, bool> _logoutNancyCommandHandler;
    INancySecurityContextFactory _nancySecurityContextFactory;
    Browser _browser;
    UsersModule _sut;
    User _authenticatedUser;

    [SetUp]
    public virtual void SetUp() {
      _getCurrentUserQueryHandler = _getCurrentUserQueryHandler.Fake();
      _getCurrentUserNancyHandler = new FakeNancyHandler<GetCurrentUserRequest, User>(_getCurrentUserQueryHandler);
      _loginCommandHandler = _loginCommandHandler.Fake();
      _loginNancyCommandHandler = new FakeNancyHandler<LoginCommand, bool>(_loginCommandHandler);
      _logoutCommandHandler = _logoutCommandHandler.Fake();
      _logoutNancyCommandHandler = new FakeNancyHandler<LogoutCommand, bool>(_logoutCommandHandler);
      _nancySecurityContextFactory = _nancySecurityContextFactory.Fake();
      _sut = new UsersModule(_getCurrentUserNancyHandler, _loginNancyCommandHandler, _logoutNancyCommandHandler, _nancySecurityContextFactory);
      _bootstrapper = new CustomBootstrapper(with => {
        with.Module(_sut);
        with.RootPathProvider(new InvoiceGenRootPathProvider());
      });
      _browser = new Browser(_bootstrapper, to => to.Accept(new MediaRange("application/json")));

      _authenticatedUser = new User {
        GivenName = "John",
        LastName = "Doe",
        Login = new Login {Value = "JDoe"},
        Password = new Password {
          Value = "P@$$w0rd",
          IsEncrypted = false
        }
      };
    }

    [Test]
    public void ConstuctorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }

    void ConfigureSecurityContextFactory_ToReturn(ISecurityContext securityContext) {
      A.CallTo(() => _nancySecurityContextFactory.Create(A<NancyContext>._))
       .Returns(securityContext);
    }
  }
}