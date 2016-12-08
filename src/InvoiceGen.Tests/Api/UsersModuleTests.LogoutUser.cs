using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Security;
using FakeItEasy;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api {
  public partial class UsersModuleTests {
    public class LogoutUser : UsersModuleTests {
      string _validPath;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _bootstrapper.AuthenticatedUser = _authenticatedUser;
        _validPath = "api/user/logout";
      }

      [Test]
      public void DoesNotAcceptUnauthorisedRequests() {
        _bootstrapper.AuthenticatedUser = null;
        var response = Post();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
      }

      [Test]
      public void ShouldDelegateControlToInnerHandler() {
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);
        var expectedCommand = new LogoutCommand {
          SecurityContext = securityContext
        };

        var response = Post();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        A.CallTo(() => _logoutCommandHandler
          .Handle(A<LogoutCommand>.That.Matches(command => command.HasSamePropertyValuesAs(expectedCommand))))
         .MustHaveHappened(Repeated.Exactly.Once);
      }

      BrowserResponse Post() {
        return _browser.Post(_validPath);
      }
    }
  }
}