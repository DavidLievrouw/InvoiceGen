using DavidLievrouw.InvoiceGen.Api.Models;
using FakeItEasy;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api {
  public partial class UsersModuleTests {
    public class LoginUser : UsersModuleTests {
      string _validPath;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _bootstrapper.AuthenticatedUser = _authenticatedUser;
        _validPath = "api/user/login";
      }

      [Test]
      public void AcceptsUnauthorisedRequests() {
        _bootstrapper.AuthenticatedUser = null;
        var response = Post();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
      }

      [Test]
      public void ParsesRequestCorrectly() {
        var expectedRequest = new LoginRequest {
          Login = "JDoe",
          Password = "ThePassword"
        };

        Post();

        A.CallTo(() => _loginCommandHandler
                   .Handle(A<LoginRequest>.That.Matches(req => req.HasSamePropertyValuesAs(expectedRequest, "NancyContext"))))
         .MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void GivenUnparseableRequest_ReturnsBadRequest() {
        var response = Post("SomeInvalidJsonString");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        A.CallTo(() => _loginCommandHandler
                   .Handle(A<LoginRequest>._))
         .MustNotHaveHappened();
      }

      [Test]
      public void GivenMissingBodyInRequest_CallsInnerHandlerWithEmptyRequest() {
        var expectedRequest = new LoginRequest {
          Login = null,
          Password = null
        };

        var response = Post(null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        A.CallTo(() => _loginCommandHandler
                   .Handle(A<LoginRequest>.That.Matches(req => req.HasSamePropertyValuesAs(expectedRequest, "NancyContext"))))
         .MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void WhenHandlerReturnsErrorCode_ReturnsErrorCode() {
        _loginNancyCommandHandler.Returns(
          new Response().WithStatusCode(HttpStatusCode.InternalServerError));
        var response = Post();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
      }

      [Test]
      public void WithValidJson_ShouldDelegateControlToInnerHandler() {
        var expectedRequest = new LoginRequest {
          Login = "JDoe",
          Password = "ThePassword"
        };

        var response = Post();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        A.CallTo(() => _loginCommandHandler
                   .Handle(A<LoginRequest>.That.Matches(req => req.HasSamePropertyValuesAs(expectedRequest, "NancyContext"))))
         .MustHaveHappened(Repeated.Exactly.Once);
        Assert.That(_loginNancyCommandHandler.GetCallCount(), Is.EqualTo(1));
      }

      BrowserResponse Post(string body = ValidJsonString) {
        return _browser.Post(_validPath,
                             with => { with.Body(body, "application/json"); });
      }

      const string ValidJsonString = "{ 'Login':'JDoe', 'Password':'ThePassword' }";
    }
  }
}