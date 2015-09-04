using DavidLievrouw.InvoiceGen.Api.Handlers;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  [TestFixture]
  public class GetCurrentUserRequestValidatorTests {
    GetCurrentUserRequestValidator _sut;

    [SetUp]
    public void SetUp() {
      _sut = new GetCurrentUserRequestValidator();
    }

    [Test]
    public void NullValue_IsInvalid() {
      var actualResult = _sut.Validate((GetCurrentUserRequest) null);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullContext_IsInvalid() {
      var input = new GetCurrentUserRequest {
        SecurityContext = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void ValidRequest_IsValid() {
      var input = new GetCurrentUserRequest {
        SecurityContext = new FakeSecurityContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}