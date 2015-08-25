using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.App {
  [TestFixture]
  public partial class AppModuleTests {
    [TestFixture]
    public class GetLoginView : AppModuleTests {
      string _validPath;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _validPath = "/";
      }

      [Test]
      public void ShouldReturnView() {
        var response = Get();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var bodyString = response.Body.AsString();
        Assert.That(string.IsNullOrWhiteSpace(bodyString), Is.False);
      }

      BrowserResponse Get() {
        return Get(_validPath);
      }

      BrowserResponse Get(string path) {
        return _browser.Get(path);
      }
    }
  }
}