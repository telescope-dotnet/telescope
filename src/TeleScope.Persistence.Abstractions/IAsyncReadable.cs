using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TeleScope.Persistence.Abstractions
{
	public interface IAsyncReadable : IReadable
	{
		Task ReadAsync<T>();
	}
}
