using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Configuration;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.Utils;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api {
  [TestFixture]
  public partial class UsersModuleTests {
    CustomBootstrapper _bootstrapper;
    IQueryHandler<GetCurrentUserRequest, User> _getCurrentUserQueryHandler;
    ICommandHandler<LoginRequest> _loginCommandHandler;
    FakeNancyQueryHandler<GetCurrentUserRequest, User> _getCurrentUserNancyQueryHandler;
    FakeNancyCommandHandler<LoginRequest> _loginNancyCommandHandler;
    Browser _browser;
    UsersModule _sut;
    User _authenticatedUser;

    [SetUp]
    public virtual void SetUp() {
      _getCurrentUserQueryHandler = _getCurrentUserQueryHandler.Fake();
      _getCurrentUserNancyQueryHandler = new FakeNancyQueryHandler<GetCurrentUserRequest, User>(_getCurrentUserQueryHandler);
      _loginCommandHandler = _loginCommandHandler.Fake();
      _loginNancyCommandHandler = new FakeNancyCommandHandler<LoginRequest>(_loginCommandHandler);
      _sut = new UsersModule(_getCurrentUserNancyQueryHandler, _loginNancyCommandHandler);
      _bootstrapper = new CustomBootstrapper(with => {
        with.Module(_sut);
        with.RootPathProvider(new InvoiceGenRootPathProvider());
      });
      _browser = new Browser(_bootstrapper, to => to.Accept(new MediaRange("application/json")));

      _authenticatedUser = new User {
        GivenName = "John",
        LastName = "Doe",
        Login = new Login { Value = "JDoe"},
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
  }
}