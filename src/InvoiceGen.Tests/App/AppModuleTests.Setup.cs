using DavidLievrouw.InvoiceGen.Configuration;
using DavidLievrouw.Utils.ForTesting.DotNet;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.App {
  [TestFixture]
  public partial class AppModuleTests {
    AppModule _sut;
    Browser _browser;
    AppBootstrapper _bootstrapper;

    [SetUp]
    public virtual void SetUp() {
      _sut = new AppModule();
      _bootstrapper = new AppBootstrapper(
        with => {
          with.Module(_sut);
          with.RootPathProvider(new InvoiceGenRootPathProvider());
        });
      _browser = new Browser(_bootstrapper, to => to.Accept(new MediaRange("text/html")));
    }

    [Test]
    public void ConstructorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }
  }
}