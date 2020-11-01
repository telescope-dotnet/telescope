using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Connectors.Abstractions.Events
{
	public delegate void ConnectorErrorEventHandler(object sender, ConnectorErrorEventArgs e);

	public class ConnectorErrorEventArgs : ConnectorEventArgs
	{

		// -- properties

		public Exception Exception { get; private set; }

		// -- constructors

		public ConnectorErrorEventArgs(Exception ex) : base()
		{
			Exception = ex;
		}

		public ConnectorErrorEventArgs(Exception ex, string name) : this(ex)
		{
			Exception = ex;
			Name = name;
		}
	}
}
