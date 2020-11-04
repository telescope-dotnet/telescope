using System;
using System.Collections.Generic;
using System.Text;
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

		public SiemensConnectorFailedEventArgs(Exception ex, int resultCode, string result, string name)
			: base(ex, result, name) 
		{
			ResultCode = resultCode;
		}
	}
}
