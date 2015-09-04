using System;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security.AspNet {
  [TestFixture]
  public class AspNetSessionTests {
    [Test]
    public void ConstructorTests() {
      Assert.Throws<ArgumentNullException>(() => new AspNetSession(null));
    }

    [Test]
    public void WhenSettingAnItem_CanAccessTheValueAfterwards() {
      var testObj = new Login {Value = "SomeLogin"};
      var aspSession = new HttpContextBuilder()
        .New()
        .WithSession()
        .Build();
      var sut = new AspNetSession(aspSession.Session);

      sut["SomeKey"] = testObj;
      var actual = sut["SomeKey"];

      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.EqualTo(testObj));
    }
  }
}