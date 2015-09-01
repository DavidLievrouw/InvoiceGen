using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;

namespace DavidLievrouw.InvoiceGen.Api {
  public class DynamicModelBinder : IModelBinder {
    public object Bind(NancyContext context, Type modelType, object instance, BindingConfig configuration, params string[] blackList) {
      var data = GetDataFields(context);
      var model = DynamicDictionary.Create(data);
      return model;
    }

    static IDictionary<string, object> GetDataFields(NancyContext context) {
      return Merge(new IDictionary<string, string>[] {
        ConvertDynamicDictionary(context.Request.Form),
        ConvertDynamicDictionary(context.Request.Query),
        ConvertDynamicDictionary(context.Parameters)
      });
    }

    static IDictionary<string, object> Merge(IEnumerable<IDictionary<string, string>> dictionaries) {
      var output =
        new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

      foreach (var kvp in dictionaries.Where(d => d != null).SelectMany(dictionary => dictionary.Where(kvp => !output.ContainsKey(kvp.Key)))) {
        output.Add(kvp.Key, kvp.Value);
      }

      return output;
    }

    static IDictionary<string, string> ConvertDynamicDictionary(DynamicDictionary dictionary) {
      return dictionary.GetDynamicMemberNames().ToDictionary(
        memberName => memberName,
        memberName => (string) dictionary[memberName]);
    }

    public bool CanBind(Type modelType) {
      return modelType == typeof(DynamicDictionary);
    }
  }
}