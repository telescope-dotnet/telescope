using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Connectors.Abstractions.Events
{
	public delegate void ConnectorFailedEventHandler(object sender, ConnectorFailedEventArgs e);

	public class ConnectorFailedEventArgs : ConnectorEventArgs
	{

		// -- properties

		public Exception Exception { get; private set; }

		public string Message { get; private set; }

		// -- constructors

		public ConnectorFailedEventArgs(Exception ex, string name) : base()
		{
			Exception = ex;
			Message = ex.Message;
			Name = name;
		}

		public ConnectorFailedEventArgs(string name, string message) : base()
		{
			Message = message;
			Name = name;
		}

		public ConnectorFailedEventArgs(Exception ex, string name, string message) : base()
		{
			Exception = ex;
			Message = message;
			Name = name;
		}
	}
}
