using System;
namespace DapperClient
{
	public class SQLiteQueryObject : IQueryObject
	{
		public SQLiteQueryObject(string commandText,object parameters = null)
		{
			Sql = commandText;
			Params = parameters;
		}

		public string Sql { get; private set; }


		public object Params { get; private set; }
	}
}
