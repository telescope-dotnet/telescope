using System.IO;

namespace TeleScope.Persistence.Abstractions
{
	/// <summary>
	/// This interface provides a generic approach to write generic data to a file based data sink. The interface inherits from <seealso cref="IWritable{T}"/>
	/// </summary>
	/// <typeparam name="T">The type that is given on the application side.</typeparam>
	public interface IFileWritable<T> : IWritable<T>
	{
		/// <summary>
		/// Updates the file information to change the file sink after instanciation process.
		/// </summary>
		/// <param name="fileInfo">The file information object.</param>
		/// <returns>The calling instancec.</returns>
		IFileWritable<T> Update(FileInfo fileInfo);
	}
}
