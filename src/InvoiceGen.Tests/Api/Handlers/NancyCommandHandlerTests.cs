using System;
using AssertExLib;
using DavidLievrouw.Utils;
using FakeItEasy;
using FluentValidation;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  [TestFixture]
  public class NancyCommandHandlerTests {
    INancyModule _module;
    ICommandHandler<string> _innerHandler;
    IBinder _defaultBinder;
    Func<string> _bindingFunc;
    NancyCommandHandler<string> _sut;

    [SetUp]
    public void SetUp() {
      _module = _module.Fake();
      _bindingFunc = _bindingFunc.Fake();
      _innerHandler = _innerHandler.Fake();
      _sut = new NancyCommandHandler<string>(_innerHandler);

      // Override the default binder (that is called by the module.Bind extension method), with a fake one
      _defaultBinder = _defaultBinder.Fake();
      var defaultModelBinderLocator = A.Fake<IModelBinderLocator>();
      A.CallTo(() => defaultModelBinderLocator.GetBinderForType(A<Type>._, A<NancyContext>._)).Returns(_defaultBinder);
      _module.ModelBinderLocator = defaultModelBinderLocator;
    }

    [Test]
    public void ConstructorTests() {
      Assert.Throws<ArgumentNullException>(() => new NancyCommandHandler<string>(null));
    }

    public class Handle : NancyCommandHandlerTests {
      [Test]
      public void GivenNullModule_Throws() {
        AssertEx.TaskThrows<ArgumentNullException>(() => _sut.Handle(null));
      }

      [Test]
      public void WhenDefaultBinderThrows_ReturnsBadRequest() {
        var exception = new InvalidOperationException();
        A.CallTo(() => _defaultBinder.Bind(A<NancyContext>._, A<Type>._, A<object>._, A<BindingConfig>._, A<string[]>._))
          .Throws(exception);

        var actual = _sut.Handle(_module).Result;

        Assert.That(actual, Is.InstanceOf<Negotiator>());
        Assert.That(((Negotiator)actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        A.CallTo(() => _innerHandler.Handle(A<string>._))
          .MustNotHaveHappened();
      }

      [Test]
      public void WhenHandlerThrowsValidationException_ReturnsBadRequest() {
        var validationException = new ValidationException(Model.ValidationFailure.Errors);
        ConfigureDefaultBindingFunc_ToReturn("The request!");
        A.CallTo(() => _innerHandler.Handle(A<string>._))
          .Throws(validationException);

        var actual = _sut.Handle(_module).Result;
        Assert.That(actual, Is.InstanceOf<Negotiator>());
        Assert.That(((Negotiator)actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        A.CallTo(() => _innerHandler.Handle(A<string>._))
          .MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void WhenHandlerThrowsException_ReturnsInternalServerError() {
        var exception = new InvalidOperationException();
        ConfigureDefaultBindingFunc_ToReturn("The request!");
        A.CallTo(() => _innerHandler.Handle(A<string>._))
          .Throws(exception);

        var actual = _sut.Handle(_module).Result;

        Assert.That(actual, Is.InstanceOf<Negotiator>());
        Assert.That(((Negotiator)actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        A.CallTo(() => _innerHandler.Handle(A<string>._))
          .MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void WhenHandlerSucceeds_ReturnsOK() {
        ConfigureDefaultBindingFunc_ToReturn("The request!");

        var actual = _sut.Handle(_module).Result;

        Assert.That(actual, Is.InstanceOf<Negotiator>());
        Assert.That(((Negotiator)actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        A.CallTo(() => _innerHandler.Handle("The request!"))
          .MustHaveHappened(Repeated.Exactly.Once);
      }

      void ConfigureDefaultBindingFunc_ToReturn(string result) {
        A.CallTo(() => _defaultBinder.Bind(A<NancyContext>._, A<Type>._, A<object>._, A<BindingConfig>._, A<string[]>._))
          .Returns(result);
      }
    }

    public class HandleWithBindingFunc : NancyCommandHandlerTests {
      [Test]
      public void GivenNullModule_Throws() {
        AssertEx.TaskThrows<ArgumentNullException>(() => _sut.Handle(null, _bindingFunc));
      }

      [Test]
      public void GivenNullBindingFuncModule_Throws() {
        AssertEx.TaskThrows<ArgumentNullException>(() => _sut.Handle(_module, null));
      }

      [Test]
      public void WhenBindingFuncThrows_ReturnsBadRequest() {
        var exception = new InvalidOperationException();
        A.CallTo(() => _bindingFunc()).Throws(exception);

        var actual = _sut.Handle(_module, _bindingFunc).Result;

        Assert.That(actual, Is.InstanceOf<Negotiator>());
        Assert.That(((Negotiator)actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        A.CallTo(() => _innerHandler.Handle(A<string>._))
          .MustNotHaveHappened();
      }

      [Test]
      public void WhenHandlerThrowsValidationException_ReturnsBadRequest() {
        var validationException = new ValidationException(Model.ValidationFailure.Errors);
        A.CallTo(() => _innerHandler.Handle(A<string>._))
          .Throws(validationException);

        var actual = _sut.Handle(_module, _bindingFunc).Result;

        Assert.That(actual, Is.InstanceOf<Negotiator>());
        Assert.That(((Negotiator)actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        A.CallTo(() => _innerHandler.Handle(A<string>._))
          .MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void WhenHandlerThrowsException_ReturnsInternalServerError() {
        var exception = new InvalidOperationException();
        A.CallTo(() => _innerHandler.Handle(A<string>._)).Throws(exception);

        var actual = _sut.Handle(_module, _bindingFunc).Result;

        Assert.That(actual, Is.InstanceOf<Negotiator>());
        Assert.That(((Negotiator)actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        A.CallTo(() => _innerHandler.Handle(A<string>._))
          .MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void WhenHandlerSucceeds_ReturnsOK() {
        ConfigureBindingFunc_ToReturn("The request!");

        var actual = _sut.Handle(_module, _bindingFunc).Result;

        Assert.That(actual, Is.InstanceOf<Negotiator>());
        Assert.That(((Negotiator)actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        A.CallTo(() => _innerHandler.Handle("The request!"))
          .MustHaveHappened(Repeated.Exactly.Once);
      }

      void ConfigureBindingFunc_ToReturn(string result) {
        A.CallTo(() => _bindingFunc())
          .Returns(result);
      }
    }
  }
}