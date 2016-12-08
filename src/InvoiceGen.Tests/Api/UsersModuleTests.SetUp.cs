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
    ApiBootstrapper _bootstrapper;
    IHandler<GetCurrentUserRequest, User> _getCurrentUserHandler;
    IHandler<LoginCommand, bool> _loginHandler;
    IHandler<LogoutCommand, bool> _logoutHandler;
    INancySecurityContextFactory _nancySecurityContextFactory;
    Browser _browser;
    UsersModule _sut;
    User _authenticatedUser;

    [SetUp]
    public virtual void SetUp() {
      _getCurrentUserHandler = _getCurrentUserHandler.Fake();
      _loginHandler = _loginHandler.Fake();
      _logoutHandler = _logoutHandler.Fake();
      _nancySecurityContextFactory = _nancySecurityContextFactory.Fake();
      _sut = new UsersModule(_getCurrentUserHandler, _loginHandler, _logoutHandler, _nancySecurityContextFactory);
      _bootstrapper = new ApiBootstrapper(with => {
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