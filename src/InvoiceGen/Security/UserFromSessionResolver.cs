﻿using DavidLievrouw.InvoiceGen.Domain;

namespace DavidLievrouw.InvoiceGen.Security {
  public class UserFromSessionResolver : IUserFromSessionResolver {
    public User ResolveUser(ISession session) {
      return session == null
        ? null
        : session[Constants.SessionKeyForUser] as User;
    }
  }
}