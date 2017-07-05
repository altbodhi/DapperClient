using System;
using System.Collections.Generic;

namespace ProbaDapperClient
{
	public class Category
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public List<Item> Items { get; set; } = new List<Item>();
		public Category() { }
		public void AddItem(Item item)
		{
			item.Category = this;
			Items.Add(item);
		}
		public Category(string name)
		{
			Name = name;
		}
		public override string ToString()
		{
			return string.Format("[Category: Id={0}, Name={1}]", Id, Name);
		}
	}
}
