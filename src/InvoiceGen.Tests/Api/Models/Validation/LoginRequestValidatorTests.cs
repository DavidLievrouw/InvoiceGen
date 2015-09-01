using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  [TestFixture]
  public class LoginRequestValidatorTests {
    LoginRequestValidator _sut;

    [SetUp]
    public void SetUp() {
      _sut = new LoginRequestValidator();
    }

    [Test]
    public void NullValue_IsInvalid() {
      var actualResult = _sut.Validate((LoginRequest) null);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullLogin_IsInvalid() {
      var input = new LoginRequest {
        Login = null,
        Password = "ThePassword"
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullPassword_IsInvalid() {
      var input = new LoginRequest {
        Login = "TheLogin",
        Password = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void EmptyLogin_IsValid() {
      var input = new LoginRequest {
        Login = string.Empty,
        Password = "ThePassword"
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }

    [Test]
    public void EmptyPassword_IsValid() {
      var input = new LoginRequest {
        Login = "TheLogin",
        Password = string.Empty
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }

    [Test]
    public void ValidRequest_IsValid() {
      var input = new LoginRequest {
        Login = "TheLogin",
        Password = "ThePassword"
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}