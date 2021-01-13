using System.Collections.Generic;

namespace TeleScope.Persistence.Abstractions
{
	public interface IReadable<out T>
	{
		// -- methods

		IEnumerable<T> Read();
	}
}
