using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Persistence.Abstractions.Crude
{
	public interface IReadable
	{
		T Read<T>();
		T Read<T>(params object[] parameters);
	}
}
