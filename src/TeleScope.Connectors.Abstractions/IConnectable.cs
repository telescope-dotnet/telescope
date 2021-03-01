using TeleScope.Connectors.Abstractions.Events;

namespace TeleScope.Connectors.Abstractions
{
	/// <summary>
	/// This interface provides basic methods, properties and events to build a connection with any external service.
	/// </summary>
	public interface IConnectable
	{
		// -- events

		/// <summary>
		/// The event is invoked when the `Connect` method has finished successfully.
		/// </summary>
		event ConnectorEventHandler Connected;

		/// <summary>
		/// The event is invoked when the `Disconnect` method has finished successfully.
		/// </summary>
		event ConnectorEventHandler Disconnected;

		/// <summary>
		/// The event is invoked when a type specific method or action has finished successfully.
		/// </summary>
		event ConnectorCompletedEventHandler Completed;

		/// <summary>
		/// The event is invoked when any method or action has finished with a failure.
		/// </summary>
		event ConnectorFailedEventHandler Failed;

		// -- properties

		/// <summary>
		/// Gets the state if the connection is established or not.
		/// </summary>
		bool IsConnected { get; }

		// -- methods

		/// <summary>
		/// Connects to an external service.
		/// </summary>
		/// <returns>The calling instance.</returns>
		IConnectable Connect();

		/// <summary>
		/// Disconnects from an external service.
		/// </summary>
		/// <returns>The calling instance.</returns>
		IConnectable Disconnect();
	}
}
