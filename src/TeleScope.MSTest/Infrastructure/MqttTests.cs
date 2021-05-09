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
		IMqttConnectable mqtt;
		private bool isConnected;
		private MqttSetup setup;

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();

			if (mqtt != null)
			{
				mqtt.DisconnectAsync();
				mqtt = null;
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

			var receiver = new MqttConnector(new MqttSetup
			{
				ClientID = $"telescope-receiver-{ts}",
				Topics = new List<string>
				{
					topic
				}
			});
			receiver.MessageReceived += (o, e) =>
			{
				log.Info($"Message received from '{e.Name}': '{e.Message}'");
				received = true;
			};
			receiver.Failed += (o, e) =>
			{
				log.Critical(e.Exception, e.Message, e);
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
				log.Info("Transmission completed");
				transmitted = true;
			};
			transmitter.Failed += (o, e) =>
			{
				log.Critical(e.Exception, e.Message, e);
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
			log.Trace($"Stopwatch: {watch.ElapsedMilliseconds}");

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
			mqtt.Connect();
			Assert.IsTrue(mqtt.IsConnected, $"mqtt {mqtt} should be connected");

			// disconnect: act & assert
			mqtt.Disconnect();
			Assert.IsFalse(mqtt.IsConnected, $"mqtt {mqtt} should be disconnected");
		}

		[TestMethod]
		public async Task ConnectAndDisconnectMqttAsync()
		{
			// arrange
			SetupClient();

			// connect: act & assert
			await ConnectAsync();
			Assert.IsTrue(isConnected, $"mqtt {mqtt} is not connected");

			// disconnect: act & assert
			await DisconnectAsync();
			Assert.IsFalse(isConnected);
		}

		// -- private methods

		private void SetupClient()
		{
			setup = new MqttSetup
			{
				Broker = "broker.hivemq.com",
				Port = 1883,
				ClientID = "TeleScope.MSTest"
			};
			mqtt = new MqttConnector(setup);
			mqtt.Connected += OnConnected;
		}

		private async Task ConnectAsync()
		{
			log.Info($"Connecting with {setup}", this);
			await mqtt.ConnectAsync();
		}

		private async Task DisconnectAsync()
		{
			log.Info($"Disconnecting from {setup}", this);
			await mqtt.DisconnectAsync().ContinueWith((task) =>
			{
				isConnected = false;
			});
		}

		// -- events

		private void OnConnected(object sender, ConnectorEventArgs e)
		{
			var mqttSender = sender as MqttConnector;
			log.Info($"Connected to '{e.Name}' with '{mqttSender.GetType()}'.", this);
			isConnected = true;
		}
	}
}
