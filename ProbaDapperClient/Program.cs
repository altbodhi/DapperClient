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

			File.Delete("demo.db");		
			var repo = new Repository();



				repo.CreateSchema();

using (var transaction = repo.StartTransaction())
			{
			var cat = new Category("Мясные продукты");
			cat.AddItem(new Item() { Name = "Колбаса" });
			cat.AddItem(new Item() { Name = "Котлеты" });
			cat.AddItem(new Item() { Name = "Пельмени" });
			repo.CreateCategory(cat);
			cat = new Category("Молочные продукты");
			cat.AddItem(new Item() { Name = "Творог" });
			cat.AddItem(new Item() { Name = "Молоко" });
			cat.AddItem(new Item() { Name = "Сметана" });
			repo.CreateCategory(cat);


				transaction.Commit();

				Console.WriteLine("done");
				Console.ReadKey();
			}
			repo.ListCategoryWithItems().ForEach(cat =>
		   {
			   Console.WriteLine(cat);
			   cat.Items.ForEach(Console.WriteLine);

		   });

		}
	}
}
