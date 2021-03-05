namespace TeleScope.Connectors.Abstractions.Events
{
	/// <summary>
	/// This delegate is used as event, when a connector has completed a certain action.
	/// </summary>
	/// <param name="sender">The instance that invokes the delegate method.</param>
	/// <param name="e">The resulting event arguments.</param>
	public delegate void ConnectorCompletedEventHandler(object sender, ConnectorCompletedEventArgs e);

	/// <summary>
	/// This argument is used for events, when a connector has completed a certain action.
	/// </summary>
	public class ConnectorCompletedEventArgs : ConnectorEventArgs
	{
		// -- propetries

		/// <summary>
		/// Gets the generic response data that should be converted on the client-side.
		/// </summary>
		public object Response { get; private set; }

		// -- constructors

		/// <summary>
		/// The default constructor sets the name of the invoking connector (or sender) and the response data.
		/// </summary>
		/// <param name="name">The name of the sender.</param>
		/// <param name="response">The generic response data of the sender, provided to the client-side.</param>
		public ConnectorCompletedEventArgs(string name, object response) : base(name)
		{
			Response = response;
		}
	}
}
