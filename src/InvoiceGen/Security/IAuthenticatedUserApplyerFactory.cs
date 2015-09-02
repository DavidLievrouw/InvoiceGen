using Nancy;

namespace DavidLievrouw.InvoiceGen.Security {
  public interface IAuthenticatedUserApplyerFactory {
    IAuthenticatedUserApplyer Create(NancyContext nancyContext);
  }
}