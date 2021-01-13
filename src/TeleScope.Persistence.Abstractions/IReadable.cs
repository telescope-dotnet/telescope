using System.Collections.Generic;

namespace TeleScope.Persistence.Abstractions
{
	public interface IReadable<Tin, Tout>
	{
		// -- methods

		IEnumerable<Tout> Read();
	}
}
