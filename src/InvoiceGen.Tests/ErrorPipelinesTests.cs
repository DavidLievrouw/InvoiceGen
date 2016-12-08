using System;
using System.Security;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using FakeItEasy;
using FluentAssertions;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen {
  [TestFixture]
  public class ErrorPipelinesTests {
    [TestFixture]
    public class HandleModelBindingExceptionPipelineTests {
      ErrorPipeline _sut;

      [SetUp]
      public void SetUp() {
        _sut = ErrorPipelines.HandleModelBindingException();
      }

      [Test]
      public void GivenExceptionIsNotModelBindingException_WhenRunningPipeline_ThenReturnsNull() {
        var result = _sut.Invoke(A.Dummy<NancyContext>(), new InvalidOperationException());
        Assert.That(result, Is.Null);
      }

      [Test]
      public void GivenExceptionIsModelBindingException_WhenRunningPipeline_ThenReturnsBadRequestWithExceptionDetails() {
        var modelBindingException = new ModelBindingException(typeof(object), new[] { new PropertyBindingException("SomeProperty", "1") });
        var expectedExceptionModel = modelBindingException.Message;

        var result = _sut.Invoke(A.Dummy<NancyContext>(), modelBindingException) as Negotiator;

        result.Should().NotBeNull();
        result.NegotiationContext.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var actualExceptionModel = result.NegotiationContext.GetModelForMediaRange("application/json") as object;
        actualExceptionModel.Should().BeAssignableTo<string>();
        ((string)actualExceptionModel).ShouldBeEquivalentTo(expectedExceptionModel);
      }
    }

    [TestFixture]
    public class HandleRequestValidationExceptionTests {
      ErrorPipeline _sut;

      [SetUp]
      public void SetUp() {
        _sut = ErrorPipelines.HandleRequestValidationException();
      }

      [Test]
      public void GivenExceptionIsNotRequestValidationException_WhenRunningPipeline_ThenReturnsNull() {
        var result = _sut.Invoke(A.Dummy<NancyContext>(), new InvalidOperationException());
        Assert.That(result, Is.Null);
      }

      [Test]
      public void GivenExceptionIsRequestValidationException_WhenRunningPipeline_ThenReturnsBadRequestWithExceptionDetails() {
        var requestValidationException = new RequestValidationException("Exception info 1");
        var expectedExceptionModel = requestValidationException.Message;

        var result = _sut.Invoke(A.Dummy<NancyContext>(), requestValidationException) as Negotiator;

        result.Should().NotBeNull();
        result.NegotiationContext.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var actualExceptionModel = result.NegotiationContext.GetModelForMediaRange("application/json") as object;
        actualExceptionModel.Should().BeAssignableTo<string>();
        ((string)actualExceptionModel).ShouldBeEquivalentTo(expectedExceptionModel);
      }
    }

    [TestFixture]
    public class HandleSecurityExceptionTests {
      ErrorPipeline _sut;

      [SetUp]
      public void SetUp() {
        _sut = ErrorPipelines.HandleSecurityException();
      }

      [Test]
      public void GivenExceptionIsNotSecurityException_WhenRunningPipeline_ThenReturnsNull() {
        var result = _sut.Invoke(A.Dummy<NancyContext>(), new InvalidOperationException());
        Assert.That(result, Is.Null);
      }

      [Test]
      public void GivenExceptionIsSecurityException_WhenRunningPipeline_ThenReturnsForbiddenWithExceptionDetails() {
        var securityException = new SecurityException("Exception info 1");
        var expectedExceptionModel = securityException.Message;

        var result = _sut.Invoke(A.Dummy<NancyContext>(), securityException) as Negotiator;

        result.Should().NotBeNull();
        result.NegotiationContext.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        var actualExceptionModel = result.NegotiationContext.GetModelForMediaRange("application/json") as object;
        actualExceptionModel.Should().BeAssignableTo<string>();
        ((string)actualExceptionModel).ShouldBeEquivalentTo(expectedExceptionModel);
      }
    }
  }
}