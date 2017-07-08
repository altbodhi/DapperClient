using System;
using DapperClient;
using Mono.Data.Sqlite;
namespace ProbaDapperClient
{
	public class ItemQueryObjects
	{
		public IQueryObject CreateTable() => new SQLiteQueryObject("create table if not exists items(name varchar(50),Category_Id Int)");
		public IQueryObject All() => new SQLiteQueryObject("Select rowid,name from items");
		public IQueryObject ByName(string name) => new SQLiteQueryObject(All().Sql + " where name like @name", new { name });
		public IQueryObject Create(Item item) => new SQLiteQueryObject("insert into items(name) values(@name);SELECT last_insert_rowid();", item);
		public IQueryObject Update(Item item) => new SQLiteQueryObject("update items set name=@name,Category_Id = @Category_Id where rowid = @rowid", new { item.RowId, item.Name,Category_Id = item.Category.RowId });
		public IQueryObject Delete(Item item) => new SQLiteQueryObject("delete from items  where rowid = @rowid", new { item.RowId});
		public ItemQueryObjects()
		{
		}

		public IQueryObject ByCategoryId(long rowId) =>  new SQLiteQueryObject($"Select rowid,name from items where category_id = @{nameof(rowId)}", new { rowId });

	}
}
