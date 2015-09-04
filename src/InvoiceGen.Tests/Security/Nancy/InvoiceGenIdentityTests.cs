using DavidLievrouw.InvoiceGen.Domain.DTO;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  [TestFixture]
  public class InvoiceGenIdentityTests {
    [Test]
    public void UserNameIsLoginValueOfSpecifiedUser() {
      var user = new User {
        Login = new Login {
          Value = "MyLogin"
        }
      };
      var actual = new InvoiceGenIdentity(user);
      Assert.That(actual.UserName, Is.EqualTo(user.Login.Value));
    }
  }
}