using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Connectors.Abstractions.Events;
using TeleScope.Connectors.Plc.Siemens;
using TeleScope.Connectors.Plc.Siemens.Events;

namespace TeleScope.MSTest.Infrastructure
{
	[TestClass]
	public class SiemensTests : TestsBase
	{
		// -- fields

		private static bool _skip;

		private S7Connector _s7;
		private S7Setup _setup;

		// -- overrides

		/// <summary>
		/// Initialize method that runs once for the entire class.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			/*
             * add the "local.runsettings" as global runsettings file, if context has no data.
             */
			_skip = bool.Parse(GetProperty(context, SKIP_PLC_TESTS));
		}

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();
			_setup = new S7Setup
			{
				Name = "SIEMENS 08/15",
				IPAddress = "0.0.0.0",
				Rack = 0,
				Slot = 2
			};
			_s7 = new S7Connector(_setup);
			_s7.Connected += Connected;
			_s7.Disconnected += Disconnected;
			_s7.Failed += Error;
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
			_s7.Connected -= Connected;
			_s7.Disconnected -= Disconnected;
			_s7.Failed -= Error;
			_s7 = null;
		}

		// -- test methods

		[TestMethod]
		public void ReadS7DataBlocks()
		{
			if (_skip)
			{
				Console.WriteLine($"Skipping ReadS7DataBlocks test...");
				return;
			}

			// arrange
			_s7.Connect();
			Assert.IsTrue(_s7.IsConnected, $"The connector should be connected to {_setup.Name}: {_setup.IPAddress}");
			Assert.IsTrue(_s7.ResultCode == 0, $"The connector returned with the Result '{_s7.Result}'.");

			// act

			/*
			 * testing possible dummy selectors
			 */
			var real1 = _s7.Select("DB123.DBD4").Read<float>();
			var real2 = _s7.Select(new S7Selector(567, 8)).Read<float>();
			var real3 = _s7.Select(new object[] { 910, 11, 0 }).Read<float>();
			var bit = _s7.Select(new int[] { 121, 31, 4 }).Read<bool>();

			// assert
			Assert.IsTrue(_s7.ResultCode == 0, $"The connector returned with the Result '{_s7.Result}'.");
		}

		[TestMethod]
		public void ConnectS7()
		{
			if (_skip)
			{
				Console.WriteLine($"Skipping ConnectS7 test...");
				return;
			}

			// arrange & act
			_s7.Connect();

			// assert
			Assert.IsTrue(_s7.IsConnected, $"The connector should be connected to {_setup.Name}: {_setup.IPAddress}");
			Assert.IsTrue(_s7.ResultCode == 0, $"The connector returned with the Result '{_s7.Result}'.");
		}

		// -- helper


		private void Error(object sender, ConnectorFailedEventArgs e)
		{
			Console.WriteLine($"Error fired from '{e.Name}'.");
			Console.WriteLine(e.Exception.Message);
		}

		private void Connected(object sender, ConnectorEventArgs e)
		{
			var sea = e as SiemensConnectorEventArgs;
			Console.WriteLine($"Connection attempt to '{sea.Name}' returned with '{sea.Result}' ({sea.ResultCode}).");

			if (sea.ResultCode != 0)
			{
				throw new Exception("The result after the connection is wrong");
			}
		}

		private void Disconnected(object sender, ConnectorEventArgs e)
		{
			var sea = e as SiemensConnectorEventArgs;
			Console.WriteLine($"Disconnection from '{sea.Name}' returned with {sea.ResultCode}.");
		}
	}
}
