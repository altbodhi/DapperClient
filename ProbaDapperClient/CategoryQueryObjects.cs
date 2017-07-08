using System;
using DapperClient;

namespace ProbaDapperClient
{
	public class CategoryQueryObjects
	{
		public IQueryObject CreateTable() => new SQLiteQueryObject("create table if not exists Categories(name varchar(50))");
		public IQueryObject All() => new SQLiteQueryObject("Select rowid ,name from Categories");
		public IQueryObject Create(Category item) => new SQLiteQueryObject("insert into Categories(name) values(@name);SELECT last_insert_rowid();", item);
		public IQueryObject Update(Category item) => new SQLiteQueryObject("update Categories set name=@name where rowid = @rowid", item);
		public IQueryObject Delete(Category item) => new SQLiteQueryObject("delete from Categories  where rowid = @RowId", new { item.RowId});
		public IQueryObject AllWithItem() => new SQLiteQueryObject(@"Select c.rowid as crowid,c.name,i.rowid,i.name,i.category_id from Categories c JOIN Items i ON i.Category_Id = c.rowid");
//c.rowid as c_id,c.name,i.rowid as i_id,i.name,i.category_id
		public CategoryQueryObjects()
		{
		}
	}
}
