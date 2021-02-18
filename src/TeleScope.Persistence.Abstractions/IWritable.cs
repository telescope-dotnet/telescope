using System.Collections.Generic;

namespace TeleScope.Persistence.Abstractions
{
	public interface IWritable<in T>
	{
		// -- properties

		bool CanCreate { get; }

		bool CanDelete { get; }

		// -- methods

		void Write(IEnumerable<T> data);
	}
}
