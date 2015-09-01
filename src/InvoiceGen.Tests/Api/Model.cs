using FluentValidation.Results;

namespace DavidLievrouw.InvoiceGen.Api {
  public class Model {
    public static ValidationResult ValidationSuccess = new ValidationResult();
    public static ValidationResult ValidationFailure = new ValidationResult(new[] {new ValidationFailure("FakeProp", "FakeErr")});
  }
}