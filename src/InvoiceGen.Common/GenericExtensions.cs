using System;
using DavidLievrouw.Utils;

namespace InvoiceGen.Common {
  public static class GenericExtensions {
    public static TResult Get<TObject, TResult>(this TObject @this, Func<TObject, TResult> getter) {
      return @this != null
        ? getter(@this)
        : default(TResult);
    }

    public static TResult DefaultIf<TResult>(this TResult input, TResult value) {
      return DefaultIf(input, value, x => x.IsNullOrDefault());
    }

    public static TResult DefaultIf<TResult>(this TResult input, TResult value, Predicate<TResult> predicate) {
      if (predicate == null) throw new ArgumentNullException("predicate");

      return predicate(input)
        ? value
        : input;
    }
  }
}