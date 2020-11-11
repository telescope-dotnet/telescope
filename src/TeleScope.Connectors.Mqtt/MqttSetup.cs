using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Connectors.Abstractions;

namespace TeleScope.Connectors.Mqtt
{
    public class MqttSetup : SetupBase
    {
        // fields

        private int _qos;

        // -- properties

        public new string Name => $"{Broker}:{Port}";

        public string Broker { get; set; }
        public int Port { get; set; }
        public string ClientID { get; set; }

        /// <summary>
        /// Message to be published by the client when he disconnects
        /// </summary>
        public string LasWill { get; set; }

        /// <summary>
        /// Gets or sets the quality of service value [0..2]
        /// </summary>
        public int QoS
        {
            get { return _qos; }
            set
            {
                if (value < 0) _qos = 0;
                else if (value > 2) _qos = 2;
                else _qos = value;
            }
        }

        /// <summary>
        /// Gets or sets the time in seconds to reconnect after a connection loss.
        /// If the value is set to the default of 0, no reconnection attempt will be done. 
        /// </summary>
        public int Reconnection { get; set; }

        /// <summary>
        /// Gets or sets the list of topic strings.
        /// </summary>
        public List<string> Topics { get; set; }

        // -- constructor

        /// <summary>
        /// Creates the default constructor with settings that connets to the public hiveMQ broker.
        /// </summary>
        public MqttSetup() : base()
        {
            Broker = "broker.hivemq.com";
            Port = 1883;
            QoS = 0;
            ClientID = $"TeleScope-Mqtt-Client_{DateTime.Now.ToUniversalTime().Ticks}";
            Topics = new List<string>();
            Reconnection = 0;
        }

        // -- methods

        public override string ToString() => $"{ClientID} @ {Broker}:{Port} with {Topics.Count} topics";

	}
}
