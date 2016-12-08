using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.InvoiceGen.Security;
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
        _getCurrentUserNancyHandler.Returns(
          new Response().WithStatusCode(HttpStatusCode.InternalServerError));
        var response = Get();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
      }

      [Test]
      public void ShouldReturnCurrentLoggedInUser() {
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);
        var expectedRequest = new GetCurrentUserRequest {
          SecurityContext = securityContext
        };

        _getCurrentUserNancyHandler.Returns(_authenticatedUser);

        var actual = Get();

        Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var actualDeserialized = actual.Body.DeserializeJson<User>();
        Assert.That(actualDeserialized.HasSamePropertyValuesAs(_authenticatedUser));

        A.CallTo(() => _getCurrentUserQueryHandler
          .Handle(A<GetCurrentUserRequest>.That.Matches(req => req.HasSamePropertyValuesAs(expectedRequest))))
         .MustHaveHappened(Repeated.Exactly.Once);
        Assert.That(_getCurrentUserNancyHandler.GetCallCount(), Is.EqualTo(1));
      }

      BrowserResponse Get() {
        return _browser.Get(_validPath);
      }
    }
  }
}