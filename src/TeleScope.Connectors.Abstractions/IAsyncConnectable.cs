using System.Threading.Tasks;

namespace TeleScope.Connectors.Abstractions
{
	/// <summary>
	/// This interface extends the capabilities of connectors with async method calls.
	/// </summary>
	public interface IAsyncConnectable
	{
		/// <summary>
		/// Connects to an external service in an asynchronous task.
		/// </summary>
		/// <returns>The executing task.</returns>
		Task ConnectAsync();

		/// <summary>
		/// Disconnects from an external service in an asynchronous task.
		/// </summary>
		/// <returns>The executing task.</returns>
		Task DisconnectAsync();
	}
}
