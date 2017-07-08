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
			client.Execute(categoryQueryObjects.CreateTable());
		}
		public void CreateItem(Item item)
		{
			item.RowId = client.Query<Int64>(itemQObjects.Create(item)).Single();
			client.Execute(itemQObjects.Update(item));
		}


		public List<Item> Items() => client.Query<Item>(itemQObjects.All()).ToList();

		public List<Item> ItemsByName(string name) => client.Query<Item>(itemQObjects.ByName(name)).ToList();
		CategoryQueryObjects categoryQueryObjects = new CategoryQueryObjects();
		public void CreateCategory(Category cat)
		{
			cat.RowId =(Int64) client.ExecuteScalar(categoryQueryObjects.Create(cat));
			foreach (var item in cat.Items)
			{
				CreateItem(item);
			}
		}

		public List<Category> ListCategoryWithItems()
		{
			var cats = client.Query<Category>(categoryQueryObjects.All()).ToList();
			foreach (var c in cats)
				client.Query<Item>(itemQObjects.ByCategoryId(c.RowId)).ToList().ForEach(c.AddItem);
			return cats;
		}

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
