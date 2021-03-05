using System.Threading.Tasks;
using TeleScope.Connectors.Abstractions;

namespace TeleScope.Connectors.Mqtt.Abstractions
{
	/// <summary>
	/// This interface provides extended methods, based on the interfaces `IConnectable` and `IAsyncConnectable` 
	/// in order to build mqtt connections to external brokers. 
	/// </summary>
	public interface IMqttConnectable : IConnectable, IAsyncConnectable
	{
		/// <summary>
		/// Subscribes to a topic at a given mqtt connection.
		/// </summary>
		/// <param name="topic">The topic that the mqtt connector will listen to.</param>
		/// <returns>The calling instance.</returns>
		IMqttConnectable Subscribe(string topic);

		/// <summary>
		/// Unsubscribes from a topic at a given mqtt connection.
		/// </summary>
		/// <param name="topic">The topic that the mqtt connector will not listen to anymore.</param>
		/// <returns></returns>
		IMqttConnectable Unsubscribe(string topic);

		/// <summary>
		/// Sends a message string asynchronously to a given broker.
		/// </summary>
		/// <param name="topic">The topic that is in use for the message.</param>
		/// <param name="message">The message string that represents the complete payload.</param>
		/// <returns>The executing task.</returns>
		Task PublishAsync(string topic, string message);

		/// <summary>
		/// Sends a message string asynchronously to a given broker.
		/// </summary>
		/// <param name="topic">The topic that is in use for the message.</param>
		/// <param name="message">The message string that represents the complete payload.</param>
		/// <param name="qualityOfService">The QOS can have the values `0: AtMostOnce`, `1: AtLeastOnce` and `2: ExactlyOnce`.</param>
		/// <returns>>The executing task.</returns>
		Task PublishAsync(string topic, string message, int qualityOfService);
	}
}
