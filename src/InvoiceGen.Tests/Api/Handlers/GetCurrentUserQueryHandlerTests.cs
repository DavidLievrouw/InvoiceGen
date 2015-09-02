﻿using System.Collections.Generic;
using System.Linq;
using DavidLievrouw.InvoiceGen.Api.Models;
using DavidLievrouw.InvoiceGen.Domain;
using DavidLievrouw.InvoiceGen.Security;
using Nancy;
using Nancy.Security;
using NUnit.Framework;

namespace DavidLievrouw.InvoiceGen.Api.Handlers {
  [TestFixture]
  public class GetCurrentUserQueryHandlerTests {
    GetCurrentUserQueryHandler _sut;

    [SetUp]
    public void SetUp() {
      _sut = new GetCurrentUserQueryHandler();
    }

    [Test]
    public void GivenContextWithoutUser_ReturnsNull() {
      var request = new GetCurrentUserRequest {
        NancyContext = new NancyContext {
          CurrentUser = null
        }
      };

      var actual = _sut.Handle(request).Result;

      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithInvalidUser_ReturnsNull() {
      var request = new GetCurrentUserRequest {
        NancyContext = new NancyContext {
          CurrentUser = new FakeUserIdentity("Polleke")
        }
      };

      var actual = _sut.Handle(request).Result;

      Assert.That(actual, Is.Null);
    }

    [Test]
    public void GivenContextWithValidUser_ReturnsUser() {
      var user = new User {Login = new Login {Value = "JohnD"}};
      var request = new GetCurrentUserRequest {
        NancyContext = new NancyContext {
          CurrentUser = new InvoiceGenIdentity(user)
        }
      };

      var actual = _sut.Handle(request).Result;

      Assert.That(actual, Is.Not.Null);
      Assert.That(actual, Is.EqualTo(user));
    }

    public class FakeUserIdentity : IUserIdentity {
      public FakeUserIdentity(string userName) {
        UserName = userName;
      }

      public string UserName { get; private set; }

      public IEnumerable<string> Claims {
        get { return Enumerable.Empty<string>(); }
      }
    }
  }
}