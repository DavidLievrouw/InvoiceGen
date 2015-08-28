using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DavidLievrouw.InvoiceGen.Data.Dapper {
  public class QueryExecutor<TConnectionFactory> : IQueryExecutor<TConnectionFactory> where TConnectionFactory : IDbConnectionFactory {
    readonly TConnectionFactory _connectionFactory;
    readonly string _sql;
    readonly object _parameters;
    readonly CommandType? _commandType;

    public QueryExecutor(TConnectionFactory connectionFactory) {
      if (connectionFactory == null) throw new ArgumentNullException("connectionFactory");
      _connectionFactory = connectionFactory;
    }

    QueryExecutor(TConnectionFactory connectionFactory, string sql, object parameters, CommandType? commandType) : this(connectionFactory) {
      if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException("sql");
      _sql = sql;
      _parameters = parameters;
      _commandType = commandType;
    }

    public IQueryExecutor<TConnectionFactory> NewQuery(string sql) {
      return new QueryExecutor<TConnectionFactory>(_connectionFactory, sql, null, null);
    }

    public IQueryExecutor<TConnectionFactory> WithParameters(object parameters) {
      return new QueryExecutor<TConnectionFactory>(_connectionFactory, _sql, parameters, _commandType);
    }

    public IQueryExecutor<TConnectionFactory> WithParameters(IEnumerable<KeyValuePair<string, object>> parameters) {
      var dynamicParameters = new DynamicParameters();
      foreach (var entry in parameters) {
        dynamicParameters.Add(entry.Key, entry.Value);
      }
      return WithParameters(dynamicParameters);
    }

    public IQueryExecutor<TConnectionFactory> WithCommandType(CommandType commandType) {
      return new QueryExecutor<TConnectionFactory>(_connectionFactory, _sql, _parameters, commandType);
    }

    public IEnumerable<TResult> Execute<TResult>() {
      return ExecuteOnConnection(c => c.Query<TResult>(_sql, _parameters, commandType: _commandType));
    }

    public IEnumerable<TResult> ExecuteWithAnonymousResult<TResult>(TResult anonymousPrototype) {
      return ExecuteOnConnection(c => c.Query<TResult>(_sql, _parameters, commandType: _commandType));
    }

    public TResult ExecuteScalar<TResult>() {
      return ExecuteOnConnection(c => c.ExecuteScalar<TResult>(_sql, _parameters, commandType: _commandType));
    }

    public int Execute() {
      return ExecuteOnConnection(c => c.Execute(_sql, _parameters, commandType: _commandType));
    }

    public Task<IEnumerable<TResult>> ExecuteAsync<TResult>() {
      return ExecuteOnConnectionAsync(c => c.QueryAsync<TResult>(_sql, _parameters, commandType: _commandType));
    }

    public Task<IEnumerable<TResult>> ExecuteWithAnonymousResultAsync<TResult>(TResult anonymousPrototype) {
      return ExecuteOnConnectionAsync(c => c.QueryAsync<TResult>(_sql, _parameters, commandType: _commandType));
    }

    public Task<TResult> ExecuteScalarAsync<TResult>() {
      return ExecuteOnConnectionAsync(c => c.ExecuteScalarAsync<TResult>(_sql, _parameters, commandType: _commandType));
    }

    public Task<int> ExecuteAsync() {
      return ExecuteOnConnectionAsync(c => c.ExecuteAsync(_sql, _parameters, commandType: _commandType));
    }

    public IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnection(c => c.Query(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TResult>(Func<TFirst, TSecond, TThird, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnection(c => c.Query(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnection(c => c.Query(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnection(c => c.Query(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnection(c => c.Query(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public IEnumerable<TResult> ExecuteWithMapper<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnection(c => c.Query(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TResult>(Func<TFirst, TSecond, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnectionAsync(c => c.QueryAsync(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TResult>(Func<TFirst, TSecond, TThird, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnectionAsync(c => c.QueryAsync(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnectionAsync(c => c.QueryAsync(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnectionAsync(c => c.QueryAsync(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnectionAsync(c => c.QueryAsync(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    public Task<IEnumerable<TResult>> ExecuteWithMapperAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> mapper, string splitOn = "Id") {
      return ExecuteOnConnectionAsync(c => c.QueryAsync(_sql, mapper, _parameters, commandType: _commandType, splitOn: splitOn));
    }

    IEnumerable<TResult> ExecuteOnConnection<TResult>(Func<IDbConnection, IEnumerable<TResult>> execute) {
      using (var connection = _connectionFactory.OpenConnection()) {
        return execute(connection);
      }
    }

    TResult ExecuteOnConnection<TResult>(Func<IDbConnection, TResult> execute) {
      using (var connection = _connectionFactory.OpenConnection()) {
        return execute(connection);
      }
    }

    async Task<IEnumerable<TResult>> ExecuteOnConnectionAsync<TResult>(Func<IDbConnection, Task<IEnumerable<TResult>>> execute) {
      using (var connection = _connectionFactory.OpenConnection()) {
        return await execute(connection);
      }
    }

    async Task<TResult> ExecuteOnConnectionAsync<TResult>(Func<IDbConnection, Task<TResult>> execute) {
      using (var connection = _connectionFactory.OpenConnection()) {
        return await execute(connection);
      }
    }
  }
}