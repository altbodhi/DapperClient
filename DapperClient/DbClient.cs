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
		public bool AutoCommit { get; private set; }
		public void SetIsolationLevel(IsolationLevel isolation)
		{
			this.isolation = isolation;
		}
		public DbClient(string providerName, string connectionString)
		{
			CreateConnection(providerName, connectionString);
		}

		private void CreateConnection(string providerName, string connectionString, bool autoCommit = false)
		{
			AutoCommit = autoCommit;
			connection?.Close();
			connection = DbProviderFactories.GetFactory(providerName).CreateConnection();
			connection.ConnectionString = connectionString;
			if (!AutoCommit)
				BeginTransaction();
		}
		public bool Connect()
		{
			if (connection.State != ConnectionState.Open)
				connection.Open();
			return (connection.ExecuteScalar("select 1;", null, transaction).ToString().CompareTo("1") == 0);
		}

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

		public IEnumerable<T> Query<T>(string commandText, object param = null)
		{
			return connection.Query<T>(commandText, param, transaction);
		}

		public int Execute(string commandText, object param = null)
		{
			return connection.Execute(commandText, param, transaction);
		}

		public void CommitTransaction() => transaction.Commit();

		public void RollbackTransaction() => transaction?.Rollback();

		public void Dispose()
		{
			transaction?.Dispose();
			connection?.Dispose();
		}
	}
}
