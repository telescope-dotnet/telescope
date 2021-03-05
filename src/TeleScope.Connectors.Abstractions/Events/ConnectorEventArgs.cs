namespace TeleScope.Connectors.Abstractions.Events
{
	/// <summary>
	/// This delegate is used as basic event, when a connector invokes an event.
	/// </summary>
	/// <param name="sender">The instance that invokes the delegate method.</param>
	/// <param name="e">The resulting event arguments.</param>
	public delegate void ConnectorEventHandler(object sender, ConnectorEventArgs e);

	/// <summary>
	/// This argument is used for basic events, when a connector invokes an basic event.
	/// </summary>
	public class ConnectorEventArgs
	{
		// -- properties

		/// <summary>
		/// Gets the name of the connector or sender as a semantic description.
		/// </summary>
		public string Name { get; protected set; }

		// -- constructors

		/// <summary>
		/// The default empty constructor creates an emtpy name.
		/// </summary>
		public ConnectorEventArgs()
		{
			Name = string.Empty;
		}

		/// <summary>
		/// Sets the name of the invoking connector (or sender).
		/// </summary>
		/// <param name="name">The name of the sender.</param>
		public ConnectorEventArgs(string name) : this()
		{
			Name = name;
		}
	}
}
