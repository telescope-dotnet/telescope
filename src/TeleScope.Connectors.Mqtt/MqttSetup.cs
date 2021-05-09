using System;
using System.Collections.Generic;

namespace TeleScope.Connectors.Mqtt
{
	/// <summary>
	/// This is a helper class to encapsulate the needed properties for a proper mqtt setup.
	/// </summary>
	public class MqttSetup
	{
		// fields

		private int quality;

		// -- properties

		/// <summary>
		/// Gets tha broker and port as string information.
		/// </summary>
		public string Name => $"{Broker}:{Port}";

		/// <summary>
		/// Gets or sets the broker for a MQTT connection.
		/// </summary>
		public string Broker { get; set; }

		/// <summary>
		/// Gets or sets the port for a MQTT connection.
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		/// Gets or sets the client id for a MQTT connection.
		/// </summary>
		public string ClientID { get; set; }

		/// <summary>
		/// Message to be published by the client when he disconnects
		/// </summary>
		public string LastWill { get; set; }

		/// <summary>
		/// Gets or sets the quality of service (QOS).
		/// The QOS can have the values `0: AtMostOnce`, `1: AtLeastOnce` and `2: ExactlyOnce`.
		/// Given values above or below the boundaries will be set to valid limits.
		/// </summary>
		public int QualityOfService
		{
			get { return quality; }
			set
			{
				if (value < 0) quality = 0;
				else if (value > 2) quality = 2;
				else quality = value;
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
			QualityOfService = 0;
			ClientID = $"TeleScope-Mqtt-Client_{DateTime.Now.ToUniversalTime().Ticks}";
			Topics = new List<string>();
			Reconnection = 0;
		}

		// -- methods

		/// <summary>
		/// Returns a formatted string with important properties.
		/// </summary>
		/// <returns>A fotmatted string.</returns>
		public override string ToString() => $"{ClientID} @ {Broker}:{Port} with {Topics.Count} topics";
	}
}
