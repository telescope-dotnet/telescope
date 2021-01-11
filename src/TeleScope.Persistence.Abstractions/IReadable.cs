using System.Collections.Generic;

namespace TeleScope.Persistence.Abstractions
{
	public interface IReadable<Tin, Tout>
	{
		// -- properties

		IParsable<Tout> IncomingParser { get; set; }

		// -- methods

		IEnumerable<Tout> Read();
	}
}
