using DavidLievrouw.InvoiceGen.Api.Models;
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
      public void WhenHandlerReturnsErrorCode_ReturnsErrorCode() {
        _logoutNancyCommandHandler.Returns(
          new Response().WithStatusCode(HttpStatusCode.InternalServerError));
        var response = Post();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
      }

      [Test]
      public void ShouldDelegateControlToInnerHandler() {
        var response = Post();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        A.CallTo(() => _logoutCommandHandler
                   .Handle(A<LogoutCommand>.That.Not.IsNull()))
         .MustHaveHappened(Repeated.Exactly.Once);
        Assert.That(_logoutNancyCommandHandler.GetCallCount(), Is.EqualTo(1));
      }

      BrowserResponse Post() {
        return _browser.Post(_validPath);
      }
    }
  }
}