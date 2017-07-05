using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dapper;

namespace DapperClient
{
	public class DbClient : IDisposable
	{
		private DbConnection connection;
		private DbTransaction transaction;
		private IsolationLevel isolation = IsolationLevel.ReadCommitted;
		private string providerName;
		private string connectionString;
		public bool AutoCommit { get; private set; }
		/// <summary>
		/// Sets the isolation level.
		/// </summary>
		/// <param name="isolation">Isolation.</param>
		public void SetIsolationLevel(IsolationLevel isolation)
		{
			this.isolation = isolation;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="T:DapperClient.DbClient"/> class.
		/// </summary>
		/// <param name="providerName">Provider name.</param>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="autoCommit">If set to <c>true</c> auto commit.</param>
		public DbClient(string providerName, string connectionString, bool autoCommit = false)
		{
			AutoCommit = autoCommit;
			this.providerName = providerName;
			this.connectionString = connectionString;
			BuildConnection();
		}
		/// <summary>
		/// Builds the connection.
		/// </summary>
		private void BuildConnection()
		{
			connection?.Close();
			connection = DbProviderFactories.GetFactory(providerName).CreateConnection();
			connection.ConnectionString = connectionString;
			if (!AutoCommit)
				BeginTransaction();
		}
		/// <summary>
		/// Reconnect this instance.
		/// </summary>
		/// <returns>The reconnect.</returns>
		public bool Reconnect()
		{
			this.Dispose();
			BuildConnection();
			return Connect();
		}
		/// <summary>
		/// Открывает соединение с БД и выполняет sql="select 1;"
		/// </summary>
		/// <returns>true if select success</returns>
		public bool Connect()
		{
			if (connection.State != ConnectionState.Open)
				connection.Open();
			return (connection.ExecuteScalar("select 1;", null, transaction).ToString().CompareTo("1") == 0);
		}
		/// <summary>
		/// Open connection and begins the transaction if autoCommit = false
		/// </summary>
		public void BeginTransaction()
		{
			if (AutoCommit)
			{
				transaction = null;
				return;
			}
			if (connection?.State != ConnectionState.Open)
				connection?.Open();
			transaction = connection?.BeginTransaction();
		}

		/// <summary>
		/// Query the specified commandText and param.
		/// </summary>
		/// <returns>The query.</returns>
		/// <param name="commandText">Command text.</param>
		/// <param name="param">object with query parameters</param>
		/// <typeparam name="T">T or dynamic</typeparam>
		public IEnumerable<T> Query<T>(string commandText, object param = null)
		{
			return connection.Query<T>(commandText, param, transaction);
		}

		public object ExecuteScalar(string commandText, object param = null)
		{
			return connection.ExecuteScalar(commandText, param, transaction);
		}
		/// <summary>
		/// Execute the specified commandText and param.
		/// </summary>
		/// <returns>The execute.</returns>
		/// <param name="commandText">Command text.</param>
		/// <param name="param">object with query parameters</param>
		public int Execute(string commandText, object param = null)
		{
			return connection.Execute(commandText, param, transaction);
		}

		/// <summary>
		/// Commits the transaction.
		/// </summary>
		public void CommitTransaction() => transaction.Commit();
		/// <summary>
		/// Rollbacks the transaction.
		/// </summary>
		public void RollbackTransaction() => transaction?.Rollback();
		/// <summary>
		/// Releases all resource used by the <see cref="T:DapperClient.DbClient"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:DapperClient.DbClient"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="T:DapperClient.DbClient"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="T:DapperClient.DbClient"/> so the garbage
		/// collector can reclaim the memory that the <see cref="T:DapperClient.DbClient"/> was occupying.</remarks>
		public void Dispose()
		{
			transaction?.Dispose();
			connection?.Dispose();
		}
	}
}
