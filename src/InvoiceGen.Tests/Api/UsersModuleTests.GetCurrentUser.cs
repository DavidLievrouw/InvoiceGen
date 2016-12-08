using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
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
      public void ShouldReturnCurrentLoggedInUser() {
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);
        var expectedRequest = new GetCurrentUserRequest {
          SecurityContext = securityContext
        };

        A.CallTo(() => _getCurrentUserHandler.Handle(A<GetCurrentUserRequest>.That.HasSamePropertyValuesAs(expectedRequest))).Returns(_authenticatedUser);

        var actual = Get();

        Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var actualDeserialized = actual.Body.DeserializeJson<User>();
        Assert.That(actualDeserialized.HasSamePropertyValuesAs(_authenticatedUser));

        A.CallTo(() => _getCurrentUserHandler
          .Handle(A<GetCurrentUserRequest>.That.Matches(req => req.HasSamePropertyValuesAs(expectedRequest))))
         .MustHaveHappened(Repeated.Exactly.Once);
      }

      BrowserResponse Get() {
        return _browser.Get(_validPath);
      }
    }
  }
}