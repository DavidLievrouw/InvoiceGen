namespace DavidLievrouw.InvoiceGen.Security {
  public interface IAuthenticatedUserApplyerFactory {
    IAuthenticatedUserApplyer Create(ISecurityContext securityContext);
  }
}