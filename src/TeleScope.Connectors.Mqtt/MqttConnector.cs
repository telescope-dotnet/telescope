﻿using System;
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
using TeleScope.Connectors.Abstractions.Extensions;
using TeleScope.Connectors.Mqtt.Abstractions;
using TeleScope.Connectors.Mqtt.Events;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace TeleScope.Connectors.Mqtt
{
	public class MqttConnector : IMqttConnectable
	{

		// -- fields 

		public event ConnectorEventHandler Connected;
		public event ConnectorEventHandler Disconnected;
		public event ConnectorCompletedEventHandler Completed;
		public event ConnectorFailedEventHandler Failed;

		public event MqttConnectorEventHandler MessageReceived;

		private ILogger _log;

		private IMqttClient _client;
		private IMqttClientOptions _options;

		private MqttSetup _setup;

		// -- properties

		public bool IsConnected => _client?.IsConnected ?? false;

		// constructors

		public MqttConnector()
		{
			_log = LoggingProvider.CreateLogger<MqttConnector>();
		}

		// -- methods

		public IConnectable Setup(SetupBase mqttSetup)
		{
			try
			{
				_setup = this.ValidateSetupOrThrow<MqttSetup>(mqttSetup);

				_options = new MqttClientOptionsBuilder()
				   .WithTcpServer(_setup.Broker, _setup.Port)
				   .WithClientId(_setup.ClientID)
				   .WithCleanSession(true)
				   .Build();

				var factory = new MqttFactory();
				_client = factory.CreateMqttClient();

				_client.UseConnectedHandler(async e => await Task.Run(() =>
				{
					// fire own event for application layer
					if (e.AuthenticateResult.ResultCode != MqttClientConnectResultCode.Success)
					{
						Failed?.Invoke(this, new ConnectorFailedEventArgs(e.AuthenticateResult.ResultCode.ToString(), this._setup.Name));
						throw new Exception($"Connection error '{e.AuthenticateResult.ResultCode.ToString()}' in {this.GetType().Name}");
					}

					Connected?.Invoke(this, new ConnectorEventArgs(_setup.Name));
					// bind all topics to the connected broker
					foreach (string topic in _setup.Topics)
					{
						SubscribeOnClient(topic);
					}
				}));

				_client.UseDisconnectedHandler(async e =>
				{
					var msg = $"Disconnected from mqtt broker '{_setup.Broker}:{_setup.Port}'.";
					_log.Trace(msg, this);
					// fire own event for application layer
					Disconnected?.Invoke(this, new ConnectorFailedEventArgs(msg, _setup.Name));

					if (_setup.Reconnection > 0)
					{
						await Task.Delay(TimeSpan.FromSeconds(_setup.Reconnection));
						try
						{
							await ConnectAsync();
						}
						catch (Exception ex)
						{
							_log.Critical(ex, $"Could not reconnect to mqtt broker '{_setup.Broker}:{_setup.Port}'.", this);
						}
					}
				});

				_client.UseApplicationMessageReceivedHandler(e =>
				{
					_log.Trace($"Mqtt received topic: {e.ApplicationMessage.Topic}", e);
					string topic = e.ApplicationMessage.Topic;
					string msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
					MessageReceived?.Invoke(this, new MqttConnectorEventArgs(topic, msg));
				});

				_log.Trace("Setup completed in {0} - {1}", _setup.ToString(), this);
			}
			catch (ArgumentNullException ex)
			{
				_log.Error(ex, "The setup was not successfull. The setup parameter was null.");
			}
			catch (Exception ex)
			{
				_log.Error(ex, $"The setup was not successfull. The setup is of type {mqttSetup.GetType()}.");
			}

			return this;
		}

		public IConnectable Connect()
		{
			Task.Run(ConnectAsync).Wait();
			return this;
		}

		public async Task ConnectAsync()
		{
			await _client
				.ConnectAsync(_options)
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
						args = new ConnectorFailedEventArgs(t.Exception, msg, _setup?.Name);
					}
					else
					{
						args = new ConnectorFailedEventArgs(msg, _setup?.Name);
					}

					Failed?.Invoke(this, args);
				});
		}

		public IConnectable Disconnect()
		{
			Task.Run(DisconnectAsync).Wait();
			return this;
		}

		public async Task DisconnectAsync()
		{
			await _client.DisconnectAsync();
		}

		public Task PublishAsync(string topic, string message)
		{
			return PublishAsync(topic, message, _setup.QoS);
		}

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

			await _client.PublishAsync(msg).ContinueWith((t) =>
			{
				_log.Trace($"Publish finished with status '{t.Status}'.", t);
				if (t.IsFaulted)
				{
					Failed?.Invoke(this, new ConnectorFailedEventArgs(t.Exception, _setup.Name));
				}
				else
				{
					Completed?.Invoke(this, new ConnectorCompletedEventArgs(t.Result.ReasonString, _setup.Name));
				}
			});
		}

		public IMqttConnectable Subscribe(string topic)
		{
			_setup.Topics.Add(topic);
			_setup.Topics = _setup.Topics.Distinct().ToList();

			if (IsConnected)
			{
				SubscribeOnClient(topic);
			}
			return this;
		}

		public IMqttConnectable UnSubscribe(string topic)
		{
			_setup.Topics.Remove(topic);
			_client.UnsubscribeAsync(topic);
			return this;
		}

		// -- helper

		private void SubscribeOnClient(string topic)
		{
			_client.SubscribeAsync(
				new MqttTopicFilterBuilder()
					.WithQualityOfServiceLevel(GetQoS(_setup.QoS))
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
