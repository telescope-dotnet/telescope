using TeleScope.Connectors.Abstractions.Events;

namespace TeleScope.Connectors.Mqtt.Events
{
	public delegate void MqttConnectorEventHandler(object sender, MqttConnectorEventArgs e);

	public class MqttConnectorEventArgs : ConnectorEventArgs
	{
		// -- propetries

		public string Message { get; private set; }

		// -- constructors

		public MqttConnectorEventArgs(string topic, string message) : base(topic)
		{
			Message = message;
		}

	}
}