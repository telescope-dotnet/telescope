using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Protocol;
using TeleScope.Connectors.Abstractions;
using TeleScope.Connectors.Abstractions.Events;
using TeleScope.Connectors.Mqtt.Abstractions;
using TeleScope.Connectors.Mqtt.Events;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace TeleScope.Connectors.Mqtt
{
	/// <summary>
	/// This class implements the `IMqttConnectable` interface and uses the The library [MQTTnet](https://github.com/chkr1011/MQTTnet) internally.
	/// </summary>
	public class MqttConnector : IMqttConnectable
	{

		// -- fields

		private readonly ILogger<MqttConnector> log;
		private IMqttClient client;
		private IMqttClientOptions options;
		private readonly MqttSetup setup;

		// -- events

		/// <summary>
		/// The event is invoked when the `Connect` method has finished successfully.
		/// </summary>
		public event ConnectorEventHandler Connected;
		/// <summary>
		/// The event is invoked when the `Disconnect` method has finished successfully.
		/// </summary>
		public event ConnectorEventHandler Disconnected;
		/// <summary>
		/// The event is invoked when the `PublishAsync` methods have finished successfully.
		/// </summary>
		public event ConnectorCompletedEventHandler Completed;
		/// <summary>
		/// The event is invoked when any method or action has finished with a failure.
		/// </summary>
		public event ConnectorFailedEventHandler Failed;
		/// <summary>
		/// The event is invoked when the subscription to a topic has received a message.
		/// </summary>
		public event MqttConnectorEventHandler MessageReceived;

		// -- properties

		/// <summary>
		/// Gets the state, if the connection is established or not.
		/// </summary>
		public bool IsConnected => client?.IsConnected ?? false;

		// -- constructors

		/// <summary>
		/// The constructor instanciates the internal logging, 
		/// stores the setup configuration and prepares the internal mqtt client.
		/// </summary>
		/// <param name="setup">The setup for the mqtt client.</param>
		public MqttConnector(MqttSetup setup)
		{
			log = LoggingProvider.CreateLogger<MqttConnector>();
			this.setup = setup ?? throw new ArgumentNullException(nameof(setup));
			Setup();
		}

		// -- methods

		/// <summary>
		/// Connects the internal Mqtt client with the prepared options and
		/// invokes the Failed event if an error occures.
		/// </summary>
		/// <returns>The calling instance.</returns>
		public IConnectable Connect()
		{
			Task.Run(ConnectAsync).Wait();
			return this;
		}

		/// <summary>
		/// Connects the internal Mqtt client asynchronously with the prepared options and
		/// invokes the Failed event if an error occures.
		/// </summary>
		/// <returns>The executing task.</returns>
		public async Task ConnectAsync()
		{
			await client
				.ConnectAsync(options)
				.ContinueWith(t =>
				{
					if (!t.IsFaulted)
					{
						return;
					}

					var msg = "Connection failed";
					var args = default(ConnectorFailedEventArgs);

					if (t.Exception != null)
					{
						args = new ConnectorFailedEventArgs(t.Exception, setup.Name, msg);
					}
					else
					{
						args = new ConnectorFailedEventArgs(setup.Name, msg);
					}

					Failed?.Invoke(this, args);
				});
		}

		/// <summary>
		/// Disconnects the internal Mqtt client.
		/// </summary>
		/// <returns>The calling instance.</returns>
		public IConnectable Disconnect()
		{
			Task.Run(DisconnectAsync).Wait();
			return this;
		}

		/// <summary>
		/// Disconnects the internal Mqtt client asynchronously.
		/// </summary>
		/// <returns>The executing task.</returns>
		public async Task DisconnectAsync()
		{
			await client.DisconnectAsync();
		}

		/// <summary>
		/// Sends a message string asynchronously to a given broker.
		/// </summary>
		/// <param name="topic">The topic of the message.</param>
		/// <param name="message">The message that will be sent.</param>
		/// <returns>The executing task.</returns>
		public Task PublishAsync(string topic, string message)
		{
			return PublishAsync(topic, message, setup.QualityOfService);
		}

		/// <summary>
		/// Sends a message string asynchronously to a given broker.
		/// </summary>
		/// <param name="topic">The topic of the message.</param>
		/// <param name="message">The message that will be sent.</param>
		/// <param name="qualityOfService">The level of quality that will be used for sending.</param>
		/// <returns>The executing task.</returns>
		public async Task PublishAsync(string topic, string message, int qualityOfService)
		{
			MqttApplicationMessage msg = new MqttApplicationMessageBuilder()
			   .WithTopic(topic)
			   .WithPayload(message)
			   .WithRetainFlag(false)
			   .Build();

			if (qualityOfService >= 0 &&
				qualityOfService <= 2)
			{
				msg.QualityOfServiceLevel = (MqttQualityOfServiceLevel)qualityOfService;
			}
			else
			{
				msg.QualityOfServiceLevel = MqttQualityOfServiceLevel.AtMostOnce;
			}

			await client.PublishAsync(msg).ContinueWith((t) =>
			{
				log.Trace($"Publish finished with status '{t.Status}'.", t);
				if (t.IsFaulted)
				{
					Failed?.Invoke(this, new ConnectorFailedEventArgs(t.Exception, setup.Name));
				}
				else
				{
					Completed?.Invoke(this, new ConnectorCompletedEventArgs(setup.Name, t.Result.ReasonString));
				}
			});
		}

		/// <summary>
		/// Subscribes to a new topic that the client will listen to.
		/// </summary>
		/// <param name="topic">The topic of interest.</param>
		/// <returns>The calling instance.</returns>
		public IMqttConnectable Subscribe(string topic)
		{
			setup.Topics.Add(topic);
			setup.Topics = setup.Topics.Distinct().ToList();

			if (IsConnected)
			{
				SubscribeOnClient(topic);
			}
			return this;
		}

		/// <summary>
		/// Unsubscribes from a topic that the client will no longer listen to.
		/// </summary>
		/// <param name="topic">The topic that will be removed.</param>
		/// <returns>The calling instance.</returns>
		public IMqttConnectable Unsubscribe(string topic)
		{
			setup.Topics.Remove(topic);
			client.UnsubscribeAsync(topic);
			return this;
		}

		// -- helper

		private void Setup()
		{
			try
			{
				options = new MqttClientOptionsBuilder()
				   .WithTcpServer(setup.Broker, setup.Port)
				   .WithClientId(setup.ClientID)
				   .WithCleanSession(true)
				   .Build();

				var factory = new MqttFactory();
				client = factory.CreateMqttClient();

				AddConnectionHandler();
				AddDisconnectionHandler();
				AddMessageReceivedHandler();

				log.Trace("Setup completed in {0} - {1}", setup.ToString(), this);
			}
			catch (ArgumentNullException ex)
			{
				log.Error(ex, "The setup was not successfull. The setup parameter was null.");
			}
		}

		private void AddConnectionHandler()
		{
			client.UseConnectedHandler(async e => await Task.Run(() =>
			{

				if (e.ConnectResult.ResultCode != MqttClientConnectResultCode.Success)
				{
					Failed?.Invoke(this,
						new ConnectorFailedEventArgs(setup.Name, e.ConnectResult.ResultCode.ToString()));
				}

				Connected?.Invoke(this, new ConnectorEventArgs(setup.Name));

				foreach (string topic in setup.Topics)
				{
					SubscribeOnClient(topic);
				}
			}));
		}

		private void AddDisconnectionHandler()
		{
			client.UseDisconnectedHandler(async e =>
			{
				var msg = $"Disconnected from mqtt broker '{setup.Name}'.";
				log.Trace(msg, this);

				Disconnected?.Invoke(this, new ConnectorFailedEventArgs(setup.Name, msg));

				if (setup.Reconnection > 0)
				{
					await Task.Delay(TimeSpan.FromSeconds(setup.Reconnection));
					try
					{
						await ConnectAsync();
					}
					catch (Exception ex)
					{
						log.Critical(ex, $"Could not reconnect to mqtt broker '{setup.Broker}:{setup.Port}'.", this);
					}
				}
			});
		}

		private void AddMessageReceivedHandler()
		{
			client.UseApplicationMessageReceivedHandler(e =>
			{
				log.Trace($"Mqtt received topic: {e.ApplicationMessage.Topic}", e);
				string topic = e.ApplicationMessage.Topic;
				string msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
				MessageReceived?.Invoke(this, new MqttConnectorEventArgs(topic, msg));
			});
		}

		private void SubscribeOnClient(string topic)
		{
			client.SubscribeAsync(
				new MqttTopicFilterBuilder()
					.WithQualityOfServiceLevel(GetQoS(setup.QualityOfService))
					.WithTopic(topic)
					.Build());
		}

		private MqttQualityOfServiceLevel GetQoS(int qos)
		{
			switch (qos)
			{
				case 2:
					return MqttQualityOfServiceLevel.ExactlyOnce;
				case 1:
					return MqttQualityOfServiceLevel.AtLeastOnce;
				case 0:
				default:
					return MqttQualityOfServiceLevel.AtMostOnce;
			}
		}
	}
}
