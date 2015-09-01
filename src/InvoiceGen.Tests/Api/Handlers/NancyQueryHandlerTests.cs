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
  public class NancyQueryHandlerTests {
    public class NancyQueryHandlerWithoutRequestTests : NancyQueryHandlerTests {
      INancyModule _module;
      IQueryHandler<string> _innerHandler;
      NancyQueryHandler<string> _sut;

      [SetUp]
      public void SetUp() {
        _module = _module.Fake();
        _innerHandler = _innerHandler.Fake();
        _sut = new NancyQueryHandler<string>(_innerHandler);
      }

      [Test]
      public void ConstructorTests() {
        Assert.Throws<ArgumentNullException>(() => new NancyQueryHandler<string>(null));
      }

      [Test]
      public void GivenNullModule_Throws() {
        AssertEx.TaskThrows<ArgumentNullException>(() => _sut.Handle(null));
      }

      [Test]
      public void WhenHandlerThrowsValidationException_ReturnsInternalServerError() {
        A.CallTo(() => _innerHandler.Handle()).Throws(new InvalidOperationException());

        var actual = _sut.Handle(_module).Result;

        Assert.That(actual, Is.InstanceOf<Negotiator>());
        Assert.That(((Negotiator) actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        A.CallTo(() => _innerHandler.Handle())
         .MustHaveHappened(Repeated.Exactly.Once);
      }

      [Test]
      public void WhenHandlerSucceeds_ReturnsResultFromHandler() {
        const string expected = "The result!";
        ConfigureInnerHandler_ToReturn(expected);

        var actual = _sut.Handle(_module).Result;

        Assert.That(actual, Is.EqualTo(expected));
        A.CallTo(() => _innerHandler.Handle())
         .MustHaveHappened(Repeated.Exactly.Once);
      }

      void ConfigureInnerHandler_ToReturn(string result) {
        A.CallTo(() => _innerHandler.Handle())
         .Returns(result);
      }
    }

    public class NancyQueryHandlerWithRequestTests : NancyQueryHandlerTests {
      INancyModule _module;
      IQueryHandler<int, string> _innerHandler;
      IBinder _defaultBinder;
      Func<int> _bindingFunc;
      NancyQueryHandler<int, string> _sut;

      [SetUp]
      public void SetUp() {
        _module = _module.Fake();
        _bindingFunc = _bindingFunc.Fake();
        _innerHandler = _innerHandler.Fake();
        _sut = new NancyQueryHandler<int, string>(_innerHandler);

        // Override the default binder (that is called by the module.Bind extension method), with a fake one
        _defaultBinder = _defaultBinder.Fake();
        var defaultModelBinderLocator = A.Fake<IModelBinderLocator>();
        A.CallTo(() => defaultModelBinderLocator.GetBinderForType(A<Type>._, A<NancyContext>._)).Returns(_defaultBinder);
        _module.ModelBinderLocator = defaultModelBinderLocator;
      }

      [Test]
      public void ConstructorTests() {
        Assert.Throws<ArgumentNullException>(() => new NancyQueryHandler<int, string>(null));
      }

      public class Handle : NancyQueryHandlerWithRequestTests {
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
          Assert.That(((Negotiator) actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
          A.CallTo(() => _innerHandler.Handle(A<int>._))
           .MustNotHaveHappened();
        }

        [Test]
        public void WhenHandlerThrowsValidationException_ReturnsBadRequest() {
          var validationException = new ValidationException(Model.ValidationFailure.Errors);
          const int theAnswerToLifeTheUniverseAndEverything = 42;
          ConfigureDefaultBindingFunc_ToReturn(theAnswerToLifeTheUniverseAndEverything);
          A.CallTo(() => _innerHandler.Handle(theAnswerToLifeTheUniverseAndEverything))
           .Throws(validationException);

          var actual = _sut.Handle(_module).Result;
          Assert.That(actual, Is.InstanceOf<Negotiator>());
          Assert.That(((Negotiator) actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
          A.CallTo(() => _innerHandler.Handle(theAnswerToLifeTheUniverseAndEverything))
           .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void WhenHandlerThrowsException_ReturnsInternalServerError() {
          var exception = new InvalidOperationException();
          const int theAnswerToLifeTheUniverseAndEverything = 42;
          ConfigureDefaultBindingFunc_ToReturn(theAnswerToLifeTheUniverseAndEverything);
          A.CallTo(() => _innerHandler.Handle(theAnswerToLifeTheUniverseAndEverything))
           .Throws(exception);

          var actual = _sut.Handle(_module).Result;

          Assert.That(actual, Is.InstanceOf<Negotiator>());
          Assert.That(((Negotiator) actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
          A.CallTo(() => _innerHandler.Handle(theAnswerToLifeTheUniverseAndEverything))
           .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void WhenHandlerSucceeds_ReturnsOK() {
          const int theAnswerToLifeTheUniverseAndEverything = 42;
          const string expectedResult = "forty-two!";
          ConfigureDefaultBindingFunc_ToReturn(theAnswerToLifeTheUniverseAndEverything);
          ConfigureInnerHandler_ToReturn(expectedResult);

          var actual = _sut.Handle(_module).Result;

          Assert.That(actual, Is.InstanceOf<string>());
          Assert.That(actual, Is.EqualTo(expectedResult));
          A.CallTo(() => _innerHandler.Handle(theAnswerToLifeTheUniverseAndEverything))
           .MustHaveHappened(Repeated.Exactly.Once);
        }

        void ConfigureDefaultBindingFunc_ToReturn(int result) {
          A.CallTo(() => _defaultBinder.Bind(A<NancyContext>._, A<Type>._, A<object>._, A<BindingConfig>._, A<string[]>._))
           .Returns(result);
        }
      }

      public class HandleWithBindingFunc : NancyQueryHandlerWithRequestTests {
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
          Assert.That(((Negotiator) actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
          A.CallTo(() => _innerHandler.Handle(A<int>._))
           .MustNotHaveHappened();
        }

        [Test]
        public void WhenHandlerThrowsValidationException_ReturnsBadRequest() {
          var validationException = new ValidationException(Model.ValidationFailure.Errors);
          A.CallTo(() => _innerHandler.Handle(A<int>._))
           .Throws(validationException);

          var actual = _sut.Handle(_module, _bindingFunc).Result;

          Assert.That(actual, Is.InstanceOf<Negotiator>());
          Assert.That(((Negotiator) actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
          A.CallTo(() => _innerHandler.Handle(A<int>._))
           .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void WhenHandlerThrowsException_ReturnsInternalServerError() {
          var exception = new InvalidOperationException();
          A.CallTo(() => _innerHandler.Handle(A<int>._)).Throws(exception);

          var actual = _sut.Handle(_module, _bindingFunc).Result;

          Assert.That(actual, Is.InstanceOf<Negotiator>());
          Assert.That(((Negotiator) actual).NegotiationContext.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
          A.CallTo(() => _innerHandler.Handle(A<int>._))
           .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void WhenHandlerSucceeds_ReturnsOK() {
          const int theAnswerToLifeTheUniverseAndEverything = 42;
          const string expectedResult = "forty-two!";
          ConfigureBindingFunc_ToReturn(theAnswerToLifeTheUniverseAndEverything);
          ConfigureInnerHandler_ToReturn(expectedResult);

          var actual = _sut.Handle(_module, _bindingFunc).Result;

          Assert.That(actual, Is.InstanceOf<string>());
          Assert.That(actual, Is.EqualTo(expectedResult));
          A.CallTo(() => _innerHandler.Handle(theAnswerToLifeTheUniverseAndEverything))
           .MustHaveHappened(Repeated.Exactly.Once);
        }

        void ConfigureBindingFunc_ToReturn(int result) {
          A.CallTo(() => _bindingFunc())
           .Returns(result);
        }
      }

      void ConfigureInnerHandler_ToReturn(string result) {
        A.CallTo(() => _innerHandler.Handle(A<int>._))
         .Returns(result);
      }
    }
  }
}