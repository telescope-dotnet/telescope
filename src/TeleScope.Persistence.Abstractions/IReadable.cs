using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Persistence.Abstractions
{
	public interface IReadable
	{
		T Read<T>();
	}
}
