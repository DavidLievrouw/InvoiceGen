using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  [TestFixture]
  public class LoginCommandValidatorTests {
    LoginCommandValidator _sut;

    [SetUp]
    public void SetUp() {
      _sut = new LoginCommandValidator();
    }

    [Test]
    public void NullValue_IsInvalid() {
      var actualResult = _sut.Validate((LoginCommand) null);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullLogin_IsInvalid() {
      var input = new LoginCommand {
        Login = null,
        Password = "ThePassword",
        NancyContext = new NancyContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullNancyContext_IsInvalid() {
      var input = new LoginCommand {
        Login = "TheLogin",
        Password = "ThePassword",
        NancyContext = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullPassword_IsInvalid() {
      var input = new LoginCommand {
        Login = "TheLogin",
        Password = null,
        NancyContext = new NancyContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void EmptyLogin_IsValid() {
      var input = new LoginCommand {
        Login = string.Empty,
        Password = "ThePassword",
        NancyContext = new NancyContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }

    [Test]
    public void EmptyPassword_IsValid() {
      var input = new LoginCommand {
        Login = "TheLogin",
        Password = string.Empty,
        NancyContext = new NancyContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }

    [Test]
    public void ValidCommand_IsValid() {
      var input = new LoginCommand {
        Login = "TheLogin",
        Password = "ThePassword",
        NancyContext = new NancyContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}