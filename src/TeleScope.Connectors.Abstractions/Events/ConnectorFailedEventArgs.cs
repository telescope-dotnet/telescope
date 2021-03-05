using System;

namespace TeleScope.Connectors.Abstractions.Events
{
	/// <summary>
	/// This delegate is used as event, when a connector has failed a certain action.
	/// </summary>
	/// <param name="sender">The instance that invokes the delegate method.</param>
	/// <param name="e">The resulting event arguments.</param>
	public delegate void ConnectorFailedEventHandler(object sender, ConnectorFailedEventArgs e);

	/// <summary>
	/// This argument is used for events, when a connector has failed a certain action.
	/// </summary>
	public class ConnectorFailedEventArgs : ConnectorEventArgs
	{

		// -- properties

		/// <summary>
		/// Gets the exception that may have caused an failure within an action of a connector.
		/// </summary>
		public Exception Exception { get; private set; }

		/// <summary>
		/// Gets a detailled message that has caused an failure within an action of a connector.
		/// </summary>
		public string Message { get; private set; }

		// -- constructors

		/// <summary>
		/// Sets name and the message of the exception as Message property.
		/// </summary>
		/// <param name="ex">The Exception that was catched within an action of a connector.</param>
		/// <param name="name">The name of the sender.</param>
		public ConnectorFailedEventArgs(Exception ex, string name) : base()
		{
			Exception = ex;
			Message = ex.Message;
			Name = name;
		}

		/// <summary>
		/// Sets name and the Message property.
		/// </summary>
		/// <param name="name">The name of the sender.</param>
		/// <param name="message">The message that has caused the failure within an action of a connector.</param>
		public ConnectorFailedEventArgs(string name, string message) : base()
		{
			Message = message;
			Name = name;
		}

		/// <summary>
		/// Sets name, the exception and a specific message as Message property.
		/// </summary>
		/// <param name="ex">The Exception that was catched within an action of a connector.</param>
		/// <param name="name">The name of the sender.</param>
		/// <param name="message">The message that has caused the failure within an action of a connector.</param>
		public ConnectorFailedEventArgs(Exception ex, string name, string message) : base()
		{
			Exception = ex;
			Message = message;
			Name = name;
		}
	}
}
