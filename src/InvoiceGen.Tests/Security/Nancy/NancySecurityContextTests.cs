﻿using System;
using DavidLievrouw.InvoiceGen.Api.Handlers;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.InvoiceGen.Security.AspNet;
using DavidLievrouw.InvoiceGen.Security.Nancy;
using FakeItEasy;
using Nancy;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Security {
  [TestFixture]
  public class NancySecurityContextTests {
    IAspNetSessionFromNancyContextResolver _aspNetSessionFromNancyContextResolver;
    IInvoiceGenIdentityFactory _invoiceGenIdentityFactory;
    NancyContext _nancyContext;
    NancySecurityContext _sut;

    [SetUp]
    public void SetUp() {
      _aspNetSessionFromNancyContextResolver = _aspNetSessionFromNancyContextResolver.Fake();
      _invoiceGenIdentityFactory = _invoiceGenIdentityFactory.Fake();
      _nancyContext = new NancyContext();
      _sut = new NancySecurityContext(_nancyContext, _aspNetSessionFromNancyContextResolver, _invoiceGenIdentityFactory);
    }

    [Test]
    public void ConstructorTests() {
      Assert.Throws<ArgumentNullException>(() => new NancySecurityContext(null, _aspNetSessionFromNancyContextResolver, _invoiceGenIdentityFactory));
      Assert.Throws<ArgumentNullException>(() => new NancySecurityContext(_nancyContext, null, _invoiceGenIdentityFactory));
      Assert.Throws<ArgumentNullException>(() => new NancySecurityContext(_nancyContext, _aspNetSessionFromNancyContextResolver, null));
    }

    public class SetAuthenticatedUser : NancySecurityContextTests {
      [Test]
      public void SetsUserInNancyContext() {
        var user = new User();
        var identity = new InvoiceGenIdentity(user);
        ConfigureInvoiceGenIdentityFactory_ToReturn(user, identity);

        _sut.SetAuthenticatedUser(user);
        var actual = _nancyContext.CurrentUser;

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo(identity));
      }

      [Test]
      public void SetsUserInSession() {
        var session = new FakeSession();
        ConfigureSessionFromContextResolver_ToReturn(session);
        var user = new User();

        _sut.SetAuthenticatedUser(user);
        var actual = session["IC_User"];

        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo(user));
      }

      [Test]
      public void GivenNullUser_SetsNullUserInNancyContext() {
        ConfigureInvoiceGenIdentityFactory_ToReturn(null, null);

        _sut.SetAuthenticatedUser(null);
        var actual = _nancyContext.CurrentUser;

        Assert.That(actual, Is.Null);
      }

      [Test]
      public void GivenNullUser_SetsNullUserInSession() {
        var session = new FakeSession();
        ConfigureSessionFromContextResolver_ToReturn(session);

        _sut.SetAuthenticatedUser(null);
        var actual = session["IC_User"];

        Assert.That(actual, Is.Null);
      }

      [Test]
      public void WhenNoSessionExists_Throws() {
        ConfigureSessionFromContextResolver_ToReturn(null);
        Assert.Throws<InvalidOperationException>(() => _sut.SetAuthenticatedUser(new User()));
      }
    }

    public class GetAuthenticatedUser : NancySecurityContextTests {
      [Test]
      public void WhenNoIdentityIsSetInNancyContext_ReturnsNullUser() {
        _nancyContext.CurrentUser = null;
        var actual = _sut.GetAuthenticatedUser();
        Assert.That(actual, Is.Null);
      }

      [Test]
      public void WhenInvalidIdentityIsSetInNancyContext_ReturnsNullUser() {
        _nancyContext.CurrentUser = new FakeUserIdentity("Pol");
        var actual = _sut.GetAuthenticatedUser();
        Assert.That(actual, Is.Null);
      }

      [Test]
      public void WhenIdentityIsSetInNancyContext_ReturnsUserFromIdentity() {
        var user = new User();
        var identity = new InvoiceGenIdentity(user);
        _nancyContext.CurrentUser = identity;

        var actual = _sut.GetAuthenticatedUser();
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo(user));
      }
    }

    void ConfigureSessionFromContextResolver_ToReturn(ISession session) {
      A.CallTo(() => _aspNetSessionFromNancyContextResolver.ResolveSession(A<NancyContext>._))
       .Returns(session);
    }

    void ConfigureInvoiceGenIdentityFactory_ToReturn(User user, InvoiceGenIdentity identity) {
      A.CallTo(() => _invoiceGenIdentityFactory.Create(user))
       .Returns(identity);
    }
  }
}