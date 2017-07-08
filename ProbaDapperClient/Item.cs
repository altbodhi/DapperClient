using System;
namespace ProbaDapperClient
{
	public class Item
	{
		public long RowId { get; set; }
		public string Name { get; set; }
		public Category Category { get; set; }

		public Item()
		{
		}

		public override string ToString()
		{
			return string.Format("[Item: Id={0}, Name={1}, Category={2}]", RowId, Name, Category);
		}
	}
}
