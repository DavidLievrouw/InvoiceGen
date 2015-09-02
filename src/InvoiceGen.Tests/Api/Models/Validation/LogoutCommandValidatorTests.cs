using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Models.Validation {
  [TestFixture]
  public class LogoutCommandValidatorTests {
    LogoutCommandValidator _sut;

    [SetUp]
    public void SetUp() {
      _sut = new LogoutCommandValidator();
    }

    [Test]
    public void NullValue_IsInvalid() {
      var actualResult = _sut.Validate((LogoutCommand) null);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void NullNancyContext_IsInvalid() {
      var input = new LogoutCommand {
        NancyContext = null
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.False);
    }

    [Test]
    public void ValidCommand_IsValid() {
      var input = new LogoutCommand {
        NancyContext = new NancyContext()
      };
      var actualResult = _sut.Validate(input);
      Assert.That(actualResult.IsValid, Is.True);
    }
  }
}