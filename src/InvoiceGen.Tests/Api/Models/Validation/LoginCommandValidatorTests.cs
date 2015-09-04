using DavidLievrouw.InvoiceGen.Api.Handlers;
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
        SecurityContext = new FakeSecurityContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullSecurityContext_IsInvalid() {
      var input = new LoginCommand {
        Login = "TheLogin",
        Password = "ThePassword",
        SecurityContext = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullPassword_IsInvalid() {
      var input = new LoginCommand {
        Login = "TheLogin",
        Password = null,
        SecurityContext = new FakeSecurityContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void EmptyLogin_IsValid() {
      var input = new LoginCommand {
        Login = string.Empty,
        Password = "ThePassword",
        SecurityContext = new FakeSecurityContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }

    [Test]
    public void EmptyPassword_IsValid() {
      var input = new LoginCommand {
        Login = "TheLogin",
        Password = string.Empty,
        SecurityContext = new FakeSecurityContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }

    [Test]
    public void ValidCommand_IsValid() {
      var input = new LoginCommand {
        Login = "TheLogin",
        Password = "ThePassword",
        SecurityContext = new FakeSecurityContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}