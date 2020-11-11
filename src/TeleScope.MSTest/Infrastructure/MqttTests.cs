using System;
using System.Collections.Generic;
using System.Text;
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

            _mqtt = new MqttConnector();
            _setup = new MqttSetup
            {
                Broker = "broker.hivemq.com",
                Port = 1883,
                ClientID = "TeleScope.MSTest"
            };
            _mqtt.Connected += OnConnected;
            _mqtt.Setup(_setup);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
            _mqtt.DisconnectAsync();
            _mqtt = null;
        }

        // -- test methods

        // TODO: test publish and subscribe methods

        [TestMethod]
        public void ConnectAndDisconnectMqtt()
        {
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
            // connect: act & assert
            await ConnectAsync();
            Assert.IsTrue(_isConnected, $"mqtt {_mqtt} is not connected");

            // disconnect: act & assert
            await DisconnectAsync();
            Assert.IsFalse(_isConnected);
        }



        // -- private methods

        private async Task ConnectAsync()
        {
            _log.Info($"Connecting with {_setup}", this);

            // act
            await _mqtt.ConnectAsync();
        }

        private async Task DisconnectAsync()
        {
            _log.Info($"Disconnecting from {_setup}", this);

            // act
            await _mqtt.DisconnectAsync().ContinueWith((task) =>
            {
                _isConnected = false;
            });
        }

        // -- events

        private void OnConnected(object sender, ConnectorEventArgs e)
        {
            _log.Info($"Connected to '{e.Name}'.", this);
            _isConnected = true;

            var mqtt = sender as MqttConnector;
        }
    }
}
