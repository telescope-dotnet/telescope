using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Connectors.Abstractions.Events;

namespace TeleScope.Connectors.Plc.Siemens.Events
{
	public class SiemensConnectorErrorEventArgs : ConnectorErrorEventArgs
	{
		// -- properties

		/// <summary>
		/// Gets the integer based result code of the occured action.
		/// </summary>
		public int ResultCode { get; protected set; }

		/// <summary>
		/// Gets the string representation of the result code.
		/// </summary>
		public string Result { get; protected set; }

		// -- constructors

		public SiemensConnectorErrorEventArgs(Exception ex, string name, int resultCode, string result)
			: base(ex, name)
		{
			ResultCode = resultCode;
			Result = result;
		}
	}
}
