using System;
using DapperClient;
using Mono.Data.Sqlite;
namespace ProbaDapperClient
{
	public class ItemQueryObjects
	{
		public IQueryObject CreateTable() => new SQLiteQueryObject("create table if not exists items(name varchar(50))");
		public IQueryObject All() => new SQLiteQueryObject("Select rowid as id,name from items");
		public IQueryObject ByName(string name) => new SQLiteQueryObject(All().Sql + " where name like @name", new { name });
		public IQueryObject Create(Item item) => new SQLiteQueryObject("insert into items(name) values(@name)", item);
		public ItemQueryObjects()
		{
		}
	}
}
