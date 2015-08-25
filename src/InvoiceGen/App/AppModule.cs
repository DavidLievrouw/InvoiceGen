using Nancy;

namespace DavidLievrouw.InvoiceGen.App {
  public class AppModule : NancyModule {
    public AppModule() {
      Get["/"] = parameters => View["App/Login/login"];
    }
  }
}