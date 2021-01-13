using System.Collections.Generic;

namespace TeleScope.Persistence.Abstractions
{
	public interface IWritable<Tin, Tout>
	{
		// -- properties

		bool CanCreate { get; }

		bool CanDelete { get; }


		// -- methods

		void Write(IEnumerable<Tin> data);
	}
}
