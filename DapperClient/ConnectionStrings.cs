using System.Configuration;
using System;
namespace DapperClient
{
	public class ConnectionString
	{ 
		public string ProviderName { get; private set; }
		public string String { get; private set; }
		public ConnectionString(string ProviderName,  string ConnectionString)
		{
			this.String = ConnectionString;
			this.ProviderName = ProviderName;
		}
	}
	public class ConnectionStrings
	{
		public ConnectionString this[string name]
		{
			get 
			{
				var connection = ConfigurationManager.ConnectionStrings[name];
				return new ConnectionString(connection.ProviderName, connection.ConnectionString);
			}

		}
		public string[] ConnectionStringNames()
		{
			var items = new string[ConfigurationManager.ConnectionStrings.Count];
			for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count;i++)
				items[i] = ConfigurationManager.ConnectionStrings[i].Name;
				return items;
		}


	}
}
