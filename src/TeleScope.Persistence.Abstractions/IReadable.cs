using System.Collections.Generic;

namespace TeleScope.Persistence.Abstractions
{
	/// <summary>
	/// This interface provides a minimalistic generic approach to read data from any kind of (persistent) data source.
	/// </summary>
	/// <typeparam name="T">The type that is desired on the application side.</typeparam>
	public interface IReadable<out T>
	{
		// -- methods

		/// <summary>
		/// Reads a given data source and provides a collection of type T.
		/// If there is only one data object a collection with the length one is returned.
		/// </summary>
		/// <returns>The resulting data objects of type T.</returns>
		IEnumerable<T> Read();
	}
}
