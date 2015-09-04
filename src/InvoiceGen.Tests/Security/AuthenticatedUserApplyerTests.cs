using System;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.Utils;
using FakeItEasy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class AuthenticatedUserApplyerTests {
    ISecurityContext _securityContext;
    AuthenticatedUserApplyer _sut;

    [SetUp]
    public virtual void SetUp() {
      _securityContext = _securityContext.Fake();
      _sut = new AuthenticatedUserApplyer(_securityContext);
    }

    [Test]
    public void ConstructorTests() {
      Assert.Throws<ArgumentNullException>(() => new AuthenticatedUserApplyer(null));
    }

    public class ApplyAuthenticatedUser : AuthenticatedUserApplyerTests {
      User _user;

      [SetUp]
      public override void SetUp() {
        base.SetUp();
        _user = new User();
      }

      [Test]
      public void GivenNullUser_Throws() {
        Assert.Throws<ArgumentNullException>(() => _sut.ApplyAuthenticatedUser(null));
      }

      [Test]
      public void WhenSessionDoesNotExist_Throws() {
        ConfigureSecurityContext_ToReturnSession(null);
        Assert.Throws<InvalidOperationException>(() => _sut.ApplyAuthenticatedUser(_user));
      }

      [Test]
      public void SetsUserInSession() {
        var session = new FakeSession();
        ConfigureSecurityContext_ToReturnSession(session);

        _sut.ApplyAuthenticatedUser(_user);

        Assert.That(session["user"], Is.Not.Null);
        Assert.That(session["user"], Is.EqualTo(_user));
      }

      [Test]
      public void SetsAuthenticatedUser() {
        var session = new FakeSession();
        ConfigureSecurityContext_ToReturnSession(session);
        ConfigureSecurityContext_ToReturnUser(_user);

        _sut.ApplyAuthenticatedUser(_user);
        
        Assert.That(_securityContext.AuthenticatedUser, Is.Not.Null);
        Assert.That(_securityContext.AuthenticatedUser, Is.EqualTo(_user));
        A.CallTo(_securityContext).Where(x => x.Method.Name == "set_AuthenticatedUser")
                .Where(x => x.Arguments.Get<User>(0) == _user).MustHaveHappened();
      }
    }

    public class ClearAuthenticatedUser : AuthenticatedUserApplyerTests {
      [Test]
      public void WhenSessionDoesNotExist_DoesNotThrow() {
        ConfigureSecurityContext_ToReturnSession(null);
        Assert.DoesNotThrow(() => _sut.ClearAuthenticatedUser());
      }

      [Test]
      public void WhenSessionExists_AbandonsSession_AndClearsData() {
        var session = new FakeSession();
        session["user"] = new User();
        ConfigureSecurityContext_ToReturnSession(session);

        _sut.ClearAuthenticatedUser();

        Assert.That(session.IsAbandoned);
        Assert.That(session["user"], Is.Null);
      }

      [Test]
      public void ClearsAuthenticatedUser() {
        ConfigureSecurityContext_ToReturnUser(null);

        _sut.ClearAuthenticatedUser(); 
        
        A.CallTo(_securityContext).Where(x => x.Method.Name == "set_AuthenticatedUser")
         .Where(x => x.Arguments.Get<User>(0).IsNullOrDefault()).MustHaveHappened();
      }
    }

    void ConfigureSecurityContext_ToReturnSession(ISession session) {
      A.CallTo(() => _securityContext.Session)
       .Returns(session);
    }

    void ConfigureSecurityContext_ToReturnUser(User user) {
      A.CallTo(() => _securityContext.AuthenticatedUser)
       .Returns(user);
    }
  }
}