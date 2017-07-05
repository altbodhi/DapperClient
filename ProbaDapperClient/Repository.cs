using System;
using System.Collections.Generic;
using DapperClient;
using System.Linq;

namespace ProbaDapperClient
{
	public class Repository
	{
		private DbClient client;
		public void CreateSchema()
		{
			client.BeginTransaction();
			client.Execute("create table Category( Name Varchar(150))");
			client.Execute("create table Item( Name Varchar(150),Category_Id INT REFERENCES  CATEGORY(NEWID))");
			client.CommitTransaction();
		}
		private void CreateItem(Item item)
		{
			client.Execute("INSERT INTO ITEM(NAME,CATEGORY_ID) VALUES(@NAME,@id)", new { item.Name,item.Category.Id});

		}

		public void CreateCategory(Category category)
		{
			client.BeginTransaction();
			category.Id =(long) client.ExecuteScalar("INSERT INTO CATEGORY(NAME) VALUES(@NAME);SELECT last_insert_rowid() FROM CATEGORY;", new { category.Name});
			category.Items.ForEach((obj) => {
				CreateItem(obj);
			});
			client.CommitTransaction();
		}

		public List<Item> Items()
		{
			client.BeginTransaction();
			var items = client.Query<Item>("Select rowid as Id, Name From Item").ToList();
			foreach (var item in items)
			{
				item.Category = GetCategory(client.ExecuteScalar("Select Category_Id From Item where rowid=@id", new { item.Id }));
				item.Category.AddItem(item);
			}
			client.CommitTransaction();
			return items;
		}


		private void FillItems(Category parent)
		{
			var items = client.Query<Item>("Select rowid as Id, Name From Item Where Category_id=@Id", new { parent.Id }).ToList();
			foreach (var item in items)
				parent.AddItem(item);
		}

		public Category GetCategory(object id)
		{
			return client.Query<Category>("Select rowid as Id, Name From Category where rowid=@id", new { id }).SingleOrDefault();
		}

		public List<Category> Categories()
		{
				client.BeginTransaction();
			var categories = client.Query<Category>("Select rowid as Id, Name From Category").ToList();
			foreach (var category in categories)
				FillItems(category);

			client.CommitTransaction();
			return categories;
		}

		public Repository()
		{
			var connectionString = new ConnectionStrings()["demo"];
			client = new DbClient(connectionString.ProviderName, connectionString.String);
		}
	}
}
