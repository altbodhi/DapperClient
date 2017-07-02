using System;
using System.Collections.Generic;
using DapperClient;

namespace ProbaDapperClient
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var connectionString = new ConnectionStrings()["demo"];
			var client = new DbClient(connectionString.ProviderName, connectionString.String);
			Console.WriteLine("client.Connect = "+client.Connect());
			//client.BeginTransaction();
			//client.Execute("drop table Category");
			//client.Execute("create table Category(ID INTEGER PRIMARY KEY AUTOINCREMENT,Name Varchar(150))");

			

			client.BeginTransaction();
			var query = new List<Category> (client.Query<Category>("select rowId as Id,Name from Category where Name like @name"
			                                                       , new { name="Сыр"}));
			foreach (var cat in query)
				Console.WriteLine(cat);
			Console.WriteLine(query.Count);
						client.Dispose();
			//
		}
	}
}
