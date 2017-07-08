using System;
using System.Collections.Generic;
using DapperClient;
using System.IO;

namespace ProbaDapperClient
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			
		
			var repo = new Repository();



				repo.CreateSchema();


			/*var cat = new Category("Мясные продукты");
			cat.AddItem(new Item() { Name = "Колбаса" });
			cat.AddItem(new Item() { Name = "Котлеты" });
			cat.AddItem(new Item() { Name = "Пельмени" });
			repo.CreateCategory(cat);
			cat = new Category("Молочные продукты");
			cat.AddItem(new Item() { Name = "Творог" });
			cat.AddItem(new Item() { Name = "Молоко" });
			cat.AddItem(new Item() { Name = "Сметана" });
			repo.CreateCategory(cat);
			using (var transaction = repo.StartTransaction())
			{
				for (int i = 1; i < 10000; i++)
				{
					repo.CreateItem(new Item() { Name = "Творог" });
					repo.CreateItem(new Item() { Name = "Молоко" });
					repo.CreateItem(new Item() { Name = "Сметана" });
				}
				transaction.Commit();

				Console.WriteLine("done");
				Console.ReadKey();
			}*/
			repo.ItemsByName("Молоко").ForEach(Console.WriteLine);

		}
	}
}
