using DavidLievrouw.InvoiceGen.Api.Users.Models;
using DavidLievrouw.InvoiceGen.Security;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Users.Validation {
  [TestFixture]
  public class LogoutRequestValidatorTests {
    LogoutRequestValidator _sut;

    [SetUp]
    public void SetUp() {
      _sut = new LogoutRequestValidator();
    }

    [Test]
    public void NullValue_IsInvalid() {
      var actualResult = _sut.Validate((LogoutRequest) null);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullSecurityContext_IsInvalid() {
      var input = new LogoutRequest {
        SecurityContext = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void ValidCommand_IsValid() {
      var input = new LogoutRequest {
        SecurityContext = A.Dummy<ISecurityContext>()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}