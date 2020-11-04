using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Connectors.Abstractions.Events
{
	public delegate void ConnectorCompletedEventHandler(object sender, ConnectorCompletedEventArgs e);

	public class ConnectorCompletedEventArgs : ConnectorEventArgs
	{
		// -- propetries

		public object Response { get; private set; }

		// -- constructors

		public ConnectorCompletedEventArgs(object response) : base()
		{
			Response = response;
		}

		public ConnectorCompletedEventArgs(object response, string name) : base(name)
		{
			Response = response;
		}

	}
}
