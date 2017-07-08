using System;
using System.Collections.Generic;
using DapperClient;
using System.Linq;
using System.Data;

namespace ProbaDapperClient
{
	public class Repository
	{
		private DbClient client;
		private ItemQueryObjects itemQObjects = new ItemQueryObjects();
	

		public void CreateSchema()
		{
			client.Execute(itemQObjects.CreateTable());
		}
		public void CreateItem(Item item)
		{
			var qo = itemQObjects.Create(item);
			client.Execute(qo);
		}


		public List<Item> Items() => client.Query<Item>(itemQObjects.All()).ToList();

		public List<Item> ItemsByName(string name) => client.Query<Item>(itemQObjects.ByName(name)).ToList();

		public IDbTransaction StartTransaction()
		{
			return client.StartTransaction();
		}

		public Repository()
		{
			var connectionString = new ConnectionStrings()["demo"];
			client = new DbClient(connectionString.ProviderName, connectionString.String);
		}
	}
}
