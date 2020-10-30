using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Connectors.Abstractions.Events
{
	public delegate void ConnectorEventHandler(object sender, ConnectorEventArgs e);

	public class ConnectorEventArgs
	{
		// -- properties

		/// <summary>
		/// Gets the name or the key descriptor of the remote entity.
		/// </summary>
		public string Name { get; protected set; }

		// -- constructors

		public ConnectorEventArgs()
		{
			Name = string.Empty;
		}

		public ConnectorEventArgs(string name) : this()
		{
			Name = name;
		}

	}
}
