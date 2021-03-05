using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Connectors.Abstractions.Events;
using TeleScope.Connectors.Mqtt;
using TeleScope.Connectors.Mqtt.Abstractions;
using TeleScope.Logging.Extensions;

namespace TeleScope.MSTest.Infrastructure
{
	[TestClass]
	public class MqttTests : TestsBase
	{
		IMqttConnectable _mqtt;
		private bool _isConnected;
		private MqttSetup _setup;

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();

			if (_mqtt != null)
			{
				_mqtt.DisconnectAsync();
				_mqtt = null;
			}
		}

		// -- test methods

		[TestMethod]
		public async Task PublishSubscribe()
		{
			// arrange

			var ts = DateTime.Now.Ticks;
			var topic = $"telescope/connectors/mqtt/{ts}";
			var message = "Hello TeleScope";
			var received = false;
			var transmitted = false;

			var receiver = new MqttConnector(null);

			receiver = new MqttConnector(new MqttSetup
			{
				ClientID = $"telescope-receiver-{ts}",
				Topics = new List<string>
					{
						topic
					}
			});
			receiver.MessageReceived += (o, e) =>
			{
				_log.Info($"Message received from '{e.Name}': '{e.Message}'");
				received = true;
			};
			receiver.Failed += (o, e) =>
			{
				_log.Critical(e.Exception, e.Message, e);
				Assert.Fail(e.Message);
			};

			receiver.Connect();

			Assert.IsTrue(receiver.IsConnected, "The receiver should be connected.");

			var transmitter = new MqttConnector(new MqttSetup
			{
				ClientID = $"telescope-transmitter-{ts}",
			});
			transmitter.Completed += (o, e) =>
			{
				_log.Info("Transmission completed");
				transmitted = true;
			};
			transmitter.Failed += (o, e) =>
			{
				_log.Critical(e.Exception, e.Message, e);
				Assert.Fail(e.Message);
			};

			transmitter.Connect();

			Assert.IsTrue(transmitter.IsConnected, "The transmitter should be connected.");

			// act

			await transmitter.PublishAsync(topic, message);

			// wait for assert

			var watch = new Stopwatch();
			var limit = 5000;
			var expired = false;
			watch.Start();
			do
			{
				Thread.Sleep(1);
				if (watch.ElapsedMilliseconds > limit)
				{
					expired = true;
				}
			}
			while (!received && !expired);

			watch.Stop();
			_log.Trace($"Stopwatch: {watch.ElapsedMilliseconds}");

			// assert

			Assert.IsTrue(transmitted, "Message transmission not completed");
			Assert.IsTrue(received, "Message not received");
		}

		[TestMethod]
		public void ConnectAndDisconnectMqtt()
		{
			// arrange
			SetupClient();

			// connect: act & assert
			_mqtt.Connect();
			Assert.IsTrue(_mqtt.IsConnected, $"mqtt {_mqtt} should be connected");

			// disconnect: act & assert
			_mqtt.Disconnect();
			Assert.IsFalse(_mqtt.IsConnected, $"mqtt {_mqtt} should be disconnected");
		}

		[TestMethod]
		public async Task ConnectAndDisconnectMqttAsync()
		{
			// arrange
			SetupClient();

			// connect: act & assert
			await ConnectAsync();
			Assert.IsTrue(_isConnected, $"mqtt {_mqtt} is not connected");

			// disconnect: act & assert
			await DisconnectAsync();
			Assert.IsFalse(_isConnected);
		}

		// -- private methods

		private void SetupClient()
		{		
			_setup = new MqttSetup
			{
				Broker = "broker.hivemq.com",
				Port = 1883,
				ClientID = "TeleScope.MSTest"
			};
			_mqtt = new MqttConnector(_setup);
			_mqtt.Connected += OnConnected;
		}

		private async Task ConnectAsync()
		{
			_log.Info($"Connecting with {_setup}", this);
			await _mqtt.ConnectAsync();
		}

		private async Task DisconnectAsync()
		{
			_log.Info($"Disconnecting from {_setup}", this);
			await _mqtt.DisconnectAsync().ContinueWith((task) =>
			{
				_isConnected = false;
			});
		}

		// -- events

		private void OnConnected(object sender, ConnectorEventArgs e)
		{
			var mqtt = sender as MqttConnector;
			_log.Info($"Connected to '{e.Name}' with '{mqtt.GetType()}'.", this);
			_isConnected = true;
		}
	}
}
