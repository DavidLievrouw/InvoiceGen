using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DavidLievrouw.InvoiceGen.Data.Dapper {
  public interface IQueryExecutor<TConnectionFactory> where TConnectionFactory : IDbConnectionFactory {
    IQueryExecutor<TConnectionFactory> NewQuery(string sql);

    IQueryExecutor<TConnectionFactory> WithParameters(object parameters);
    IQueryExecutor<TConnectionFactory> WithParameters(IEnumerable<KeyValuePair<string, object>> parameters);
    IQueryExecutor<TConnectionFactory> WithCommandType(CommandType commandType);

    IEnumerable<TResult> Execute<TResult>();
    TResult ExecuteScalar<TResult>();
    IEnumerable<TResult> ExecuteWithAnonymousResult<TResult>(TResult anonymousPrototype);
    int Execute();

    Task<IEnumerable<TResult>> ExecuteAsync<TResult>();
    Task<TResult> ExecuteScalarAsync<TResult>();
    Task<IEnumerable<TResult>> ExecuteWithAnonymousResultAsync<TResult>(TResult anonymousPrototype);
    Task<int> ExecuteAsync();

    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TResult>(Func<TFirst, TSecond, TThird, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> mapper, string splitOn = "Id");
    IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> mapper, string splitOn = "Id");

    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TResult>(Func<TFirst, TSecond, TThird, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> mapper, string splitOn = "Id");
    Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> mapper, string splitOn = "Id");
  }
}