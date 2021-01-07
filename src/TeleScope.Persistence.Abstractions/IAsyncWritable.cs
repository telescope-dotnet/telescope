using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TeleScope.Persistence.Abstractions
{
	public interface IAsyncWritable : IWritable
	{

		// -- methods

		Task WriteAsync<T>(T data);
	}
}
