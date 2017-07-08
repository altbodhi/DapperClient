using System;
namespace DapperClient
{
	public interface IQueryObject
	{
		string Sql { get; }
		object Params { get; }
	}

}
