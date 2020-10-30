using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Connectors.Abstractions.Events;

namespace TeleScope.Connectors.Abstractions
{
	public interface IConnectable
	{
		// -- events

		event ConnectorEventHandler AfterConnect;
		event ConnectorEventHandler AfterDisconnect;
		event ConnectorErrorEventHandler OnError;

		// -- methods

		IConnectable Setup(SetupBase setup);

		IConnectable Connect();

		IConnectable Disconnect();
	}
}
