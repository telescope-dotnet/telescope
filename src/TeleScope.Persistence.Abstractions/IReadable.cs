using System;
using System.Collections.Generic;
using System.Text;

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
