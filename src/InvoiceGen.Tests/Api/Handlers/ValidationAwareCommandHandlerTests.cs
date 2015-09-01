using System;
using AssertExLib;
using DavidLievrouw.Utils;
using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  [TestFixture]
  public class ValidationAwareCommandHandlerTests {
    [Test]
    public void ConstructorTests() {
      Assert.Throws<ArgumentNullException>(() => new ValidationAwareCommandHandler<object>(null, A.Dummy<ICommandHandler<object>>()));
      Assert.Throws<ArgumentNullException>(() => new ValidationAwareCommandHandler<object>(A.Dummy<IValidator<object>>(), null));
    }

    [Test]
    public void Handle_ValidatesFirst_ThenPassesControlToDecoratedCommandHandler() {
      var validator = A.Fake<IValidator<object>>();
      A.CallTo(() => validator.Validate(A<object>._)).Returns(new ValidationResult(new ValidationFailure[0]));

      var decoratedCommandHandler = A.Fake<ICommandHandler<object>>();

      var sut = new ValidationAwareCommandHandler<object>(validator, decoratedCommandHandler);

      var command = new object();
      var task = sut.Handle(command);
      task.Wait();

      A.CallTo(() => validator.Validate(command)).MustHaveHappened();
      A.CallTo(() => decoratedCommandHandler.Handle(command)).MustHaveHappened();
    }

    [Test]
    public void WhenValidationFails_ThenThrows_DoesNotCallDecoratedCommandHandler() {
      var validator = A.Fake<IValidator<object>>();
      A.CallTo(() => validator.Validate(A<object>._)).Returns(new ValidationResult(new[] {new ValidationFailure("MyProperty", "Some error")}));

      var decoratedCommandHandler = A.Fake<ICommandHandler<object>>();

      var sut = new ValidationAwareCommandHandler<object>(validator, decoratedCommandHandler);

      var command = new object();
      AssertEx.TaskThrows<ValidationException>(() => sut.Handle(command));

      A.CallTo(() => validator.Validate(command)).MustHaveHappened();
      A.CallTo(() => decoratedCommandHandler.Handle(A<object>._)).MustNotHaveHappened();
    }
  }
}