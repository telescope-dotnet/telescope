using TeleScope.Connectors.Abstractions.Events;

namespace TeleScope.Connectors.Mqtt.Events
{
	/// <summary>
	/// This delegate is used as event handler for mqtt conncetions.
	/// </summary>
	/// <param name="sender">The instance that invokes the delegate method.</param>
	/// <param name="e">The resulting event arguments.</param>
	public delegate void MqttConnectorEventHandler(object sender, MqttConnectorEventArgs e);

	/// <summary>
	/// This argument class is used for mqtt related events, when the connector invokes an event.
	/// </summary>
	public class MqttConnectorEventArgs : ConnectorEventArgs
	{
		// -- propetries

		/// <summary>
		/// Gets the message string of the mqtt data.
		/// </summary>
		public string Message { get; private set; }

		// -- constructors

		/// <summary>
		/// The basic constructor takes the topic and the message strings and 
		/// stores it in the properties.
		/// </summary>
		/// <param name="topic">The topic of the incoming mqtt data.</param>
		/// <param name="message">The message of the incoming mqtt data.</param>
		public MqttConnectorEventArgs(string topic, string message) : base(topic)
		{
			Message = message;
		}
	}
}