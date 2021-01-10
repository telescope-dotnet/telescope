using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Persistence.Abstractions
{
	public interface IWritable<Tin, Tout>
	{

		// -- properties

		bool CanCreate { get; }

		bool CanDelete { get; }

		IParsable<Tout> OutgoingParser { get; set; }

		// -- methods

		void Write(IEnumerable<Tin> data);
	}
}
