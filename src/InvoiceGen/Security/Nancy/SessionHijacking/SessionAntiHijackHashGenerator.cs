using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Nancy;

namespace DavidLievrouw.InvoiceGen.Security.Nancy.SessionHijacking {
  public class SessionAntiHijackHashGenerator : ISessionAntiHijackHashGenerator {
    public string GenerateHash(Request request) {
      var stringToHash = new StringBuilder();
      
      stringToHash.Append(request.UserHostAddress);
      stringToHash.Append(request.Headers.UserAgent);
      stringToHash.Append(string.Join(" ", request.Headers.AcceptLanguage.Select(lang => lang.Item2 + "_" + lang.Item1)));

      SHA1 sha = new SHA1CryptoServiceProvider();
      var hashData = sha.ComputeHash(Encoding.UTF8.GetBytes(stringToHash.ToString()));
      return Convert.ToBase64String(hashData);
    }
  }
}