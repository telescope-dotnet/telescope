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
		event ConnectorFailedEventHandler Error;

		// -- properties

		bool IsConnected { get; }

		// -- methods

		IConnectable Setup(SetupBase setup);

		IConnectable Connect();

		IConnectable Disconnect();
	}
}
