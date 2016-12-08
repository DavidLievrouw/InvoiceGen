using System;
using DavidLievrouw.InvoiceGen.Domain.DTO;
using DavidLievrouw.Utils.ForTesting.DotNet;
using DavidLievrouw.Utils.ForTesting.FakeItEasy;
using FakeItEasy;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security.Nancy {
  [TestFixture]
  public class NancyIdentityFromSessionAssignerTests {
    INancySessionFromNancyContextResolver _nancySessionFromNancyContextResolver;
    IUserFromSessionResolver _userFromSessionResolver;
    IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;
    NancyIdentityFromContextAssigner _sut;
    NancyContext _context;

    [SetUp]
    public void SetUp() {
      _nancySessionFromNancyContextResolver = _nancySessionFromNancyContextResolver.Fake();
      _userFromSessionResolver = _userFromSessionResolver.Fake();
      _invoiceGenIdentityFactory = _invoiceGenIdentityFactory.Fake();
      _sut = new NancyIdentityFromContextAssigner(
        _nancySessionFromNancyContextResolver,
        _userFromSessionResolver,
        _invoiceGenIdentityFactory);
      _context = new NancyContext();
    }

    [Test]
    public void ConstructorTests() {
      Assert.That(_sut.NoDependenciesAreOptional());
    }

    [Test]
    public void GivenNullContext_Throws() {
      Assert.Throws<ArgumentNullException>(() => _sut.AssignNancyIdentityFromContext(null));
    }

    [Test]
    public void AssignsCreatedIdentityFromUserInSession() {
      var session = A.Fake<ISession>();
      var user = new User();
      var identity = new InvoiceGenIdentity(user);
      ConfigureSessionFromContextResolver_ToReturn(session);
      ConfigureUserFromSessionResolver_ToReturn(session, user);
      ConfigureInvoiceGenIdentityFactory_ToReturn(user, identity);

      _sut.AssignNancyIdentityFromContext(_context);

      Assert.That(_context.CurrentUser, Is.Not.Null);
      Assert.That(_context.CurrentUser, Is.EqualTo(identity));
      A.CallTo(() => _nancySessionFromNancyContextResolver.ResolveSession(_context))
       .MustHaveHappened();
      A.CallTo(() => _userFromSessionResolver.ResolveUser(session))
       .MustHaveHappened();
      A.CallTo(() => _invoiceGenIdentityFactory.Create(user))
       .MustHaveHappened();
    }

    void ConfigureSessionFromContextResolver_ToReturn(ISession session) {
      A.CallTo(() => _nancySessionFromNancyContextResolver.ResolveSession(A<NancyContext>._))
       .Returns(session);
    }

    void ConfigureUserFromSessionResolver_ToReturn(ISession session, User user) {
      A.CallTo(() => _userFromSessionResolver.ResolveUser(session))
       .Returns(user);
    }

    void ConfigureInvoiceGenIdentityFactory_ToReturn(User user, InvoiceGenIdentity identity) {
      A.CallTo(() => _invoiceGenIdentityFactory.Create(user))
       .Returns(identity);
    }
  }
}