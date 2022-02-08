using System.Collections.Generic;
using TeleScope.Persistence.Abstractions.Enumerations;

namespace TeleScope.Persistence.Abstractions
{
	/// <summary>
	/// This interface provides a minimalistic generic approach to write generic data to any kind of (persistent) data sink.
	/// </summary>
	/// <typeparam name="T">The type that is given on the application side.</typeparam>
	public interface IWritable<in T>
	{
		// -- properties

		/// <summary>
		/// Gets the flags of permissions how files may be treated. 
		/// </summary>
		WritePermissions Permissions { get; }

		// -- methods

		/// <summary>
		/// Writes a collection of type T to a given data sink.
		/// If there is only one data object there is the need to provide a collection with one element.
		/// </summary>
		/// <param name="data">The application-side data collection of type T.</param>
		void Write(IEnumerable<T> data);

		/// <summary>
		/// Shall check, if the implementation has the permission handed over as parameter. 
		/// </summary>
		/// <param name="permission">The permission that will be testet.</param>
		/// <returns>True if the permission flags are present, otherwise false.</returns>
		bool HasPermission(WritePermissions permission);
	}
}
