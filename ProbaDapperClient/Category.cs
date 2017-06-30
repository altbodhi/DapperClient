using System;
namespace ProbaDapperClient
{
	public class Category
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Category() { }
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
