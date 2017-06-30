using NUnit.Framework;
using System;
using DapperClient;
namespace DapperClientTests
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void DbClient_TwistConnect()
		{
			var c = new DbClient("Mono.Data.SQLite", "Data Source = demo.db");
			c.Connect();
			c.Connect();
		}
	}
}
