using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Configuration {
  [TestFixture]
  public class CustomJsonSerializerTests {
    CustomJsonSerializer _sut;

    [TestCase("application/json")]
    [TestCase("application/JSON")]
    [TestCase("application/json-prop")]
    [TestCase("application/JSON-prop")]
    [TestCase("text/json")]
    [TestCase("text/JSON")]
    [TestCase("application/vndTEST+json")]
    [TestCase("application/vndTEST+JSON")]
    [TestCase("application/json;otherdata")]
    [TestCase("application/json; charset=utf-8")]
    public void CanSerializeJsonContentType(string contentType) {
      Assert.That(_sut.CanSerialize(contentType), Is.True);
    }

    [TestCase("image/png")]
    [TestCase("text/html")]
    public void CannotSerializeNonJsonContentType(string contentType) {
      Assert.That(_sut.CanSerialize(contentType), Is.False);
    }

    class Data {
      public string Message { get; set; }
      public int Number { get; set; }
      public bool AssertionSucceeded { get; set; }
    }

    class DataWithPrivateSetters {
      public DataWithPrivateSetters(string privateSetter) {
        PrivateSetter = privateSetter;
      }

      public string PublicSetter { get; set; }
      public string PrivateSetter { get; }
    }

    [SetUp]
    public void SetUp() {
      _sut = new CustomJsonSerializer();
    }

    [Test]
    public void CanRoundtripToJson() {
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};

      var serializedData = _sut.Serialize(originalData);
      var deserializedData = _sut.Deserialize<Data>(serializedData);

      Assert.That(deserializedData.Message, Is.EqualTo(originalData.Message));
      Assert.That(deserializedData.Number, Is.EqualTo(originalData.Number));
      Assert.That(deserializedData.AssertionSucceeded, Is.EqualTo(originalData.AssertionSucceeded));
    }

    [Test]
    public void CanSerializeJsonContent() {
      const string contentType = "application/json";
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};

      using (var memStream = new MemoryStream()) {
        Assert.DoesNotThrow(() => _sut.Serialize(contentType, originalData, memStream));
        Assert.That(memStream.Position, Is.GreaterThan(0));
      }
    }

    [Test]
    public void ConfiguresJsonExtension() {
      Assert.That(_sut.Extensions.Contains("json"), Is.True);
    }

    [Test]
    public void DeserializeNullString_Fails() {
      Assert.Throws<ArgumentNullException>(() => _sut.Deserialize<Data>(null));
    }

    [Test]
    public void RegisteredCorrectContractResolver() {
      var serializer = (JsonSerializer) _sut;
      Assert.That(serializer.ContractResolver, Is.AssignableTo<CamelCasePropertyNamesContractResolver>());
    }

    [Test]
    public void Serialize_WithInvalidJsonString_Fails() {
      const string invalidJson = "Blah Blah";
      Assert.Throws<JsonReaderException>(() => _sut.Deserialize<Data>(invalidJson));
    }

    [Test]
    public void Serialize_WithNullContentType_Fails() {
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};

      using (var memStream = new MemoryStream()) {
        Assert.Throws<ArgumentNullException>(() => _sut.Serialize(null, originalData, memStream));
      }
    }

    [Test]
    public void Serialize_WithNullOutputStream_Fails() {
      const string contentType = "text/html";
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};

      Assert.Throws<ArgumentNullException>(() => _sut.Serialize(contentType, originalData, null));
    }

    [Test]
    public void SerializeNonJsonContent_Fails() {
      const string contentType = "text/html";
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};

      using (var memStream = new MemoryStream()) {
        Assert.Throws<SerializationException>(() => _sut.Serialize(contentType, originalData, memStream));
      }
    }

    [Test]
    public void SerializesNullInputModel_ToNullJson() {
      var json = _sut.Serialize(null);
      Assert.That(json, Is.EqualTo("null"));
    }

    [Test]
    public void SerializesToCamelCaseProperties() {
      var originalData = new Data {Message = "test", Number = 42, AssertionSucceeded = true};
      const string expectedResult = @"{""message"":""test"",""number"":42,""assertionSucceeded"":true}";

      var actual = _sut.Serialize(originalData);

      Assert.That(actual, Is.EqualTo(expectedResult));
    }

    [Test]
    public void SupportsBothPublicAndPrivatePropertySetters() {
      var originalData = new DataWithPrivateSetters("privateData") {PublicSetter = "publicData"};

      var serializedData = _sut.Serialize(originalData);
      var deserializedData = _sut.Deserialize<DataWithPrivateSetters>(serializedData);

      Assert.That(deserializedData.PublicSetter, Is.EqualTo(originalData.PublicSetter));
      Assert.That(deserializedData.PrivateSetter, Is.EqualTo(originalData.PrivateSetter));
    }
  }
}