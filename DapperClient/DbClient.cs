using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dapper;

namespace DapperClient
{
	public class DbClient : IDisposable
	{
		private IDbConnection connection;
		private IDbTransaction transaction;
		private string providerName;
		private string connectionString;
		public bool AutoCommit { get; private set; }

		public DbClient(string providerName, string connectionString)
		{
			this.providerName = providerName;
			this.connectionString = connectionString;
			BuildConnection();
		}

		private void BuildConnection()
		{
			connection?.Close();
			connection = DbProviderFactories.GetFactory(providerName).CreateConnection();
			connection.ConnectionString = connectionString;
		}

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
		public IDbTransaction StartTransaction()
		{
			if (connection?.State != ConnectionState.Open)
				connection?.Open();
			transaction = connection?.BeginTransaction();
			return transaction;
		}

		private IDbTransaction GetWorkingTransaction()
		{
			if (transaction?.Connection != null)
				return transaction;
			return null;
		}

		public IEnumerable<T> Query<T>(IQueryObject queryObject)
		{
			return connection.Query<T>(queryObject.Sql, queryObject.Params, GetWorkingTransaction());
		}


		public object ExecuteScalar(IQueryObject queryObject)
		{
			return connection.ExecuteScalar(queryObject.Sql, queryObject.Params, GetWorkingTransaction());
		}

		public int Execute(IQueryObject queryObject)
		{
			return connection.Execute(queryObject.Sql, queryObject.Params, GetWorkingTransaction());
		}



		public void Dispose()
		{
			transaction?.Dispose();
			connection?.Dispose();
		}
	}
}
