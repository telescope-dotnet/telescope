namespace TeleScope.Connectors.Abstractions.Events
{
	public delegate void ConnectorCompletedEventHandler(object sender, ConnectorCompletedEventArgs e);

	public class ConnectorCompletedEventArgs : ConnectorEventArgs
	{
		// -- propetries

		public object Response { get; private set; }

		// -- constructors


		public ConnectorCompletedEventArgs(string name, object response) : base(name)
		{
			Response = response;
		}

	}
}
