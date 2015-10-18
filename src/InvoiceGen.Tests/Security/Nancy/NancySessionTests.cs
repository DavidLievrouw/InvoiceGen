using System;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using Nancy.Session;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  [TestFixture]
  public class NancySessionTests {
    [Test]
    public void ConstructorTests() {
      Assert.Throws<ArgumentNullException>(() => new NancySession(null));
    }

    [Test]
    public void WhenSettingAnItem_CanAccessTheValueAfterwards() {
      var testObj = new Login {Value = "SomeLogin"};
      var sut = new NancySession(new Session()) {
        ["SomeKey"] = testObj
      };

      var actual = sut["SomeKey"];

      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.EqualTo(testObj));
    }

    [Test]
    public void WhenAbandoningTheSession_TheItemsHaveBeenCleared() {
      var testObj = new Login {Value = "SomeLogin"};
      var sut = new NancySession(new Session()) {
        ["SomeKey"] = testObj
      };

      sut.Abandon();
      var actual = sut["SomeKey"];

      Assert.That(actual, Is.Null);
    }
  }
}