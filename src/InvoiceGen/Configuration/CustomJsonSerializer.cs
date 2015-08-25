using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Nancy;
using Nancy.IO;
using Newtonsoft.Json;

namespace DavidLievrouw.InvoiceGen.Configuration {
  public sealed class CustomJsonSerializer : JsonSerializer, ICustomJsonSerializer, ISerializer {
    public CustomJsonSerializer() {
      ContractResolver = new CamelCaseAndSupportForPrivateSettersContractResolver();
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    public string Serialize(object model) {
      using (var outputStream = new MemoryStream()) {
        Serialize(model, outputStream);
        outputStream.Seek(0, SeekOrigin.Begin);
        using (var reader = new StreamReader(outputStream)) {
          return reader.ReadToEnd();
        }
      }
    }

    public T Deserialize<T>(string jsonString) {
      if (jsonString == null) throw new ArgumentNullException("jsonString");
      using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString))) {
        using (var reader = new StreamReader(stream)) {
          using (var jsonReader = new JsonTextReader(reader)) {
            return Deserialize<T>(jsonReader);
          }
        }
      }
    }

    public bool CanSerialize(string contentType) {
      return IsJsonType(contentType);
    }

    public IEnumerable<string> Extensions {
      get { yield return "json"; }
    }

    public void Serialize<TModel>(string contentType, TModel model, Stream outputStream) {
      if (contentType == null) throw new ArgumentNullException("contentType");
      if (outputStream == null) throw new ArgumentNullException("outputStream");
      if (!CanSerialize(contentType))
        throw new SerializationException(string.Format("Content type '{0}' is not supported by this {1}.", contentType, GetType().Name));

      Serialize(model, outputStream);
    }

    void Serialize(object model, Stream outputStream) {
      using (var writer = new StreamWriter(new UnclosableStreamWrapper(outputStream))) {
        using (var jsonWriter = new JsonTextWriter(writer)) {
          Serialize(jsonWriter, model);
        }
      }
    }

    static bool IsJsonType(string contentType) {
      if (string.IsNullOrEmpty(contentType)) return false;

      var contentMimeType = contentType.Split(';')[0];
      return contentMimeType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase) ||
             contentMimeType.StartsWith("application/json-", StringComparison.InvariantCultureIgnoreCase) ||
             contentMimeType.Equals("text/json", StringComparison.InvariantCultureIgnoreCase) ||
             (
               contentMimeType.StartsWith("application/vnd", StringComparison.InvariantCultureIgnoreCase) &&
               contentMimeType.EndsWith("+json", StringComparison.InvariantCultureIgnoreCase));
    }
  }
}