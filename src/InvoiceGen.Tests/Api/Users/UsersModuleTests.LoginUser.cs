using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Security;
using DavidLievrouw.Utils.ForTesting.CompareNetObjects;
using FakeItEasy;
using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Users {
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
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);

        var expectedCommand = new LoginRequest {
          Login = "JDoe",
          Password = "ThePassword",
          SecurityContext = securityContext
        };

        Post();

        A.CallTo(() => _loginHandler
          .Handle(A<LoginRequest>.That.Matches(req => req.HasSamePropertyValuesAs(expectedCommand))))
         .MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void GivenUnparseableRequest_ReturnsBadRequest() {
        var response = Post("SomeInvalidJsonString");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        A.CallTo(() => _loginHandler
          .Handle(A<LoginRequest>._))
         .MustNotHaveHappened();
      }

      [Test]
      public void GivenMissingBodyInRequest_CallsInnerHandlerWithEmptyRequest() {
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);
        var expectedCommand = new LoginRequest {
          Login = null,
          Password = null,
          SecurityContext = securityContext
        };

        var response = Post(null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        A.CallTo(() => _loginHandler
          .Handle(A<LoginRequest>.That.Matches(req => req.HasSamePropertyValuesAs(expectedCommand))))
         .MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void WithValidJson_ShouldDelegateControlToInnerHandler() {
        var securityContext = A.Fake<ISecurityContext>();
        ConfigureSecurityContextFactory_ToReturn(securityContext);
        var expectedCommand = new LoginRequest {
          Login = "JDoe",
          Password = "ThePassword",
          SecurityContext = securityContext
        };

        var response = Post();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        A.CallTo(() => _loginHandler
          .Handle(A<LoginRequest>.That.Matches(req => req.HasSamePropertyValuesAs(expectedCommand))))
         .MustHaveHappened(Repeated.Exactly.Once);
      }

      BrowserResponse Post(string body = ValidJsonString) {
        return _browser.Post(_validPath,
          with => { with.Body(body, "application/json"); });
      }

      const string ValidJsonString = "{ 'Login':'JDoe', 'Password':'ThePassword' }";
    }
  }
}