using System;
using TeleScope.Connectors.Abstractions.Events;

namespace TeleScope.Connectors.Plc.Siemens.Events
{
	public class SiemensConnectorFailedEventArgs : ConnectorFailedEventArgs
	{
		// -- properties

		/// <summary>
		/// Gets the integer based result code of the occured action.
		/// </summary>
		public int ResultCode { get; protected set; }

		// -- constructors

		public SiemensConnectorFailedEventArgs(Exception ex, string name, int resultCode, string result)
			: base(ex, name, result)
		{
			ResultCode = resultCode;
		}
	}
}
