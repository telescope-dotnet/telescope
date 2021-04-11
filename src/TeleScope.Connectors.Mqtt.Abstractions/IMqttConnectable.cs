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
		/// Subscribes to a new topic that the client will listen to.
		/// </summary>
		/// <param name="topic">The topic of interest.</param>
		/// <returns>The calling instance.</returns>
		IMqttConnectable Subscribe(string topic);

		/// <summary>
		/// Unsubscribes from a topic that the client will no longer listen to.
		/// </summary>
		/// <param name="topic">The topic that will be removed.</param>
		/// <returns>The calling instance.</returns>
		IMqttConnectable Unsubscribe(string topic);

		/// <summary>
		/// Sends a message string asynchronously to a given broker.
		/// </summary>
		/// <param name="topic">The topic of the message.</param>
		/// <param name="message">The message that will be sent.</param>
		/// <returns>The executing task.</returns>
		Task PublishAsync(string topic, string message);

		/// <summary>
		/// Sends a message string asynchronously to a given broker.
		/// </summary>
		/// <param name="topic">The topic of the message.</param>
		/// <param name="message">The message that will be sent.</param>
		/// <param name="qualityOfService">The level of quality that will be used for sending.</param>
		/// <returns>The executing task.</returns>
		Task PublishAsync(string topic, string message, int qualityOfService);
	}
}
