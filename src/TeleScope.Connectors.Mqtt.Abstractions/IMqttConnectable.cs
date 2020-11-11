using System;
using System.Threading.Tasks;
using TeleScope.Connectors.Abstractions;

namespace TeleScope.Connectors.Mqtt.Abstractions
{
	public interface IMqttConnectable : IConnectable, IAsyncConnectable
	{

		IMqttConnectable Subscribe(string topic);

		Task PublishAsync(string topic, string message);

		Task PublishAsync(string topic, string message, int qualityOfService);
	}
}
