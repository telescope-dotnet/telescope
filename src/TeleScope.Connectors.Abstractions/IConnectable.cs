using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Connectors.Abstractions.Events;

namespace TeleScope.Connectors.Abstractions
{
	public interface IConnectable
	{
		// -- events

		event ConnectorEventHandler Connected;
		event ConnectorEventHandler Disconnected;
		event ConnectorCompletedEventHandler Completed;
		event ConnectorFailedEventHandler Failed;

		// -- properties

		bool IsConnected { get; }

		// -- methods

		IConnectable Connect();

		IConnectable Disconnect();
	}
}
