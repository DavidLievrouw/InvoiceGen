using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Configuration;
using DavidLievrouw.InvoiceGen.Domain;
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
    IQueryHandler<GetCurrentUserRequest, User> _getCurrentUserQueryHandler;
    ICommandHandler<LoginCommand> _loginCommandHandler;
    ICommandHandler<LogoutCommand> _logoutCommandHandler;
    FakeNancyQueryHandler<GetCurrentUserRequest, User> _getCurrentUserNancyQueryHandler;
    FakeNancyCommandHandler<LoginCommand> _loginNancyCommandHandler;
    FakeNancyCommandHandler<LogoutCommand> _logoutNancyCommandHandler;
    INancySecurityContextFactory _nancySecurityContextFactory;
    Browser _browser;
    UsersModule _sut;
    User _authenticatedUser;

    [SetUp]
    public virtual void SetUp() {
      _getCurrentUserQueryHandler = _getCurrentUserQueryHandler.Fake();
      _getCurrentUserNancyQueryHandler = new FakeNancyQueryHandler<GetCurrentUserRequest, User>(_getCurrentUserQueryHandler);
      _loginCommandHandler = _loginCommandHandler.Fake();
      _loginNancyCommandHandler = new FakeNancyCommandHandler<LoginCommand>(_loginCommandHandler);
      _logoutCommandHandler = _logoutCommandHandler.Fake();
      _logoutNancyCommandHandler = new FakeNancyCommandHandler<LogoutCommand>(_logoutCommandHandler);
      _nancySecurityContextFactory = _nancySecurityContextFactory.Fake();
      _sut = new UsersModule(_getCurrentUserNancyQueryHandler, _loginNancyCommandHandler, _logoutNancyCommandHandler, _nancySecurityContextFactory);
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