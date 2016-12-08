using System;
using System.Linq.Expressions;
using Nancy;
using Nancy.ModelBinding;

namespace DavidLievrouw.InvoiceGen {
  public static class NancyModuleBindingExtensions {
    public static TModel Bind<TModel>(this NancyModule module, object blacklistedProperties) {
      if (module == null) throw new ArgumentNullException(nameof(module));
      return module.Bind<TModel>((BindingConfig) null, null);
    }

    public static TModel Bind<TModel>(this NancyModule module, BindingConfig configuration) {
      if (module == null) throw new ArgumentNullException(nameof(module));
      return module.Bind<TModel>(configuration, null);
    }

    public static TModel Bind<TModel>(this NancyModule module, params Expression<Func<TModel, object>>[] blacklistedProperties) {
      if (module == null) throw new ArgumentNullException(nameof(module));
      return module.Bind((BindingConfig) null, blacklistedProperties);
    }

    public static TModel Bind<TModel>(this NancyModule module, BindingConfig configuration, params Expression<Func<TModel, object>>[] blacklistedProperties) {
      if (module == null) throw new ArgumentNullException(nameof(module));
      try {
        if ((configuration == null) && (blacklistedProperties == null)) return ModuleExtensions.Bind<TModel>(module);
        if ((configuration != null) && (blacklistedProperties == null)) return ModuleExtensions.Bind<TModel>(module, configuration);
        return configuration == null
          ? ModuleExtensions.Bind(module, blacklistedProperties)
          : ModuleExtensions.Bind(module, configuration, blacklistedProperties);
      } catch (ModelBindingException) {
        throw;
      } catch (Exception ex) {
        throw new ModelBindingException(typeof(TModel), null, ex);
      }
    }

    public static TModel Bind<TModel>(this NancyModule module, Func<TModel> customBinder) {
      if (module == null) throw new ArgumentNullException(nameof(module));
      try {
        return customBinder();
      } catch (ModelBindingException) {
        throw;
      } catch (Exception ex) {
        throw new ModelBindingException(typeof(TModel), null, ex);
      }
    }

    public static TModel Bind<TModel>(this NancyModule module, Func<TModel, TModel> postBinding) {
      return Bind(module, postBinding, (BindingConfig) null, null);
    }

    public static TModel Bind<TModel>(this NancyModule module, Func<TModel, TModel> postBinding, BindingConfig configuration) {
      return Bind(module, postBinding, configuration, null);
    }

    public static TModel Bind<TModel>(this NancyModule module, Func<TModel, TModel> postBinding, params Expression<Func<TModel, object>>[] blacklistedProperties) {
      return Bind(module, postBinding, null, blacklistedProperties);
    }

    public static TModel Bind<TModel>(this NancyModule module, Func<TModel, TModel> postBinding, BindingConfig configuration, params Expression<Func<TModel, object>>[] blacklistedProperties) {
      if (module == null) throw new ArgumentNullException(nameof(module));
      try {
        var theDefault = Bind(module, configuration, blacklistedProperties);
        return postBinding(theDefault);
      } catch (ModelBindingException) {
        throw;
      } catch (Exception ex) {
        throw new ModelBindingException(typeof(TModel), null, ex);
      }
    }
  }
}