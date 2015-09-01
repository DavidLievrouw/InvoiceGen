using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain;
using FakeItEasy;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api {
  public partial class UsersModuleTests {
    public class GetCurrentUser : UsersModuleTests {
      string _validPath;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _bootstrapper.AuthenticatedUser = _authenticatedUser;
        _validPath = "api/user";
      }

      [Test]
      public void DoesNotAcceptUnauthorisedRequests() {
        _bootstrapper.AuthenticatedUser = null;
        var response = Get();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
      }

      [Test]
      public void WhenHandlerReturnsErrorCode_ReturnsErrorCode() {
        _getCurrentUserNancyQueryHandler.Returns(
          new Response().WithStatusCode(HttpStatusCode.InternalServerError));
        var response = Get();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
      }

      [Test]
      public void ShouldReturnCurrentLoggedInUser() {
        _getCurrentUserNancyQueryHandler.Returns(_authenticatedUser);

        var actual = Get();

        Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var actualDeserialized = actual.Body.DeserializeJson<User>();
        Assert.That(actualDeserialized.HasSamePropertyValuesAs(_authenticatedUser));

        A.CallTo(() => _getCurrentUserQueryHandler
                   .Handle(A<GetCurrentUserRequest>._))
         .MustHaveHappened(Repeated.Exactly.Once);
        Assert.That(_getCurrentUserNancyQueryHandler.GetCallCount(), Is.EqualTo(1));
      }

      BrowserResponse Get() {
        return _browser.Get(_validPath);
      }
    }
  }
}