using System;
using System.IO;
using System.Web.Configuration;
using Autofac;
using DavidLievrouw.InvoiceGen.Configuration;
using DavidLievrouw.InvoiceGen.Security;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Composition {
  [TestFixture]
  public class CompositionRootTests {
    IContainer _sut;

    [SetUp]
    public virtual void SetUp() {
      var rootPathProvider = new InvoiceGenRootPathProvider();
      var webConfigFile = new FileInfo(Path.Combine(rootPathProvider.GetRootPath(), "web.config"));
      var virtualDirectoryMapping = new VirtualDirectoryMapping(webConfigFile.DirectoryName, true, webConfigFile.Name);
      var webConfigurationFileMap = new WebConfigurationFileMap();
      webConfigurationFileMap.VirtualDirectories.Add("/", virtualDirectoryMapping);
      var configuration = WebConfigurationManager.OpenMappedWebConfiguration(webConfigurationFileMap, "/");
      _sut = CompositionRoot.Compose(configuration);
    }

    [TestCase(typeof(IUserFromSessionResolver))]
    [TestCase(typeof(IInvoiceGenIdentityFactory))]
    public void ShouldBeRegistered(Type serviceType) {
      object actualResult = null;
      Assert.DoesNotThrow(() => actualResult = _sut.Resolve(serviceType));
      Assert.That(actualResult, Is.Not.Null);
      Assert.That(actualResult, Is.InstanceOf(serviceType));
    }

    [TestCase(typeof(JsonSerializer), typeof(CustomJsonSerializer))]
    [TestCase(typeof(ICustomJsonSerializer), typeof(CustomJsonSerializer))]
    public void ShouldBeRegisteredCorrectly(Type serviceType, Type instanceType) {
      object actualResult = null;
      Assert.DoesNotThrow(() => actualResult = _sut.Resolve(serviceType));
      Assert.That(actualResult, Is.Not.Null);
      Assert.That(actualResult, Is.InstanceOf(instanceType));
    }
  }
}