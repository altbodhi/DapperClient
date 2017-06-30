using System;
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
			int c = (1000*1000);
			while ((c = c - 5) > 0)
			{
				client.Execute("INSERT INTO CATEGORY(name) VALUES(@name)", new { name = "Колбаса" });
				client.Execute("INSERT INTO CATEGORY(name) VALUES(@name)", new { name = "Сыр" });
				client.Execute("INSERT INTO CATEGORY(name) VALUES(@name)", new { name = "Сметана" });
				client.Execute("INSERT INTO CATEGORY(name) VALUES(@name)", new { name = "Молоко" });
				client.Execute("INSERT INTO CATEGORY(name) VALUES(@name)", new { name = "Сахар" });
				Console.WriteLine(c);
			}
			client.CommitTransaction();
			

			client.BeginTransaction();
			var query = client.Query<Category>("select rowId as Id,Name from Category");
			foreach (var cat in query)
				Console.WriteLine(cat);
						client.Dispose();
			//
		}
	}
}
