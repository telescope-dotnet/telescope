using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Connectors.Abstractions.Events;
using TeleScope.Connectors.Plc.Siemens;
using TeleScope.Connectors.Plc.Siemens.Events;
using TeleScope.Persistence.Json;
using TeleScope.Logging.Extensions;
using System.IO;

namespace TeleScope.MSTest.Infrastructure
{
	[TestClass]
	public class SiemensTests : TestsBase
	{
		// -- fields

		private static bool skip;

		private S7Connector s7;
		private S7Setup setup;

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
			skip = bool.Parse(GetProperty(context, SKIP_PLC_TESTS));
		}

		[TestInitialize]
		public override void Arrange()
		{
			base.ArrangeLogging<SiemensTests>();
			setup = GetS7Setup();
			s7 = new S7Connector(setup);
			s7.Connected += Connected;
			s7.Disconnected += Disconnected;
			s7.Failed += Error;
		}

		[TestCleanup]
		public override void Cleanup()
		{
			s7.Connected -= Connected;
			s7.Disconnected -= Disconnected;
			s7.Failed -= Error;
			s7 = null;
		}

		// -- test methods

		[TestMethod]
		public void ReadS7DataBlocks()
		{
			if (skip)
			{
				Console.WriteLine($"Skipping ReadS7DataBlocks test...");
				return;
			}

			// arrange
			s7.Connect();
			Assert.IsTrue(s7.IsConnected, $"The connector should be connected to {setup.Name}: {setup.IPAddress}");
			Assert.IsTrue(s7.ResultCode == 0, $"The connector returned with the Result '{s7.Result}'.");

			// act

			/*
			 * testing possible selectors
			 */

			var xPos = s7.Select("DB652.DBD82").Read<float>();
			var zPos = s7.Select(new S7Selector(652, 86)).Read<float>();
			var cPos = s7.Select(new object[] { 652, 90, 0 }).Read<float>();
			var uPos = s7.Select(new int[] { 652, 94, 0 }).Read<float>();
			var mode1 = s7.Select("DB652.DBX684.4").Read<bool>();
			var mode2 = s7.Select("DB652.DBX684.5").Read<bool>();
			var mode3 = s7.Select("DB652.DBX684.6").Read<bool>();

			LogData(new object[] {
				$"{nameof(xPos)} {xPos}",
				$"{nameof(zPos)} {zPos}",
				$"{nameof(cPos)} {cPos}",
				$"{nameof(uPos)} {uPos}",
				$"{nameof(mode1)} {mode1}",
				$"{nameof(mode2)} {mode2}",
				$"{nameof(mode3)} {mode3}"
			});

			// assert
			Assert.IsTrue(s7.ResultCode == 0, $"The connector returned with the Result '{s7.Result}'.");

		}

		[TestMethod]
		public void ConnectS7()
		{
			if (skip)
			{
				Console.WriteLine($"Skipping ConnectS7 test...");
				return;
			}

			// arrange & act
			s7.Connect();

			// assert
			Assert.IsTrue(s7.IsConnected, $"The connector should be connected to {setup.Name}: {setup.IPAddress}");
			Assert.IsTrue(s7.ResultCode == 0, $"The connector returned with the Result '{s7.Result}'.");
		}

		// -- helper

		private S7Setup GetS7Setup()
		{
			S7Setup s7Setup;
			try
			{
				var fileInfo = new FileInfo("App_Data/s7setup.json");
				var json = new JsonStorage<S7Setup>(
					new JsonStorageSetup(fileInfo));
				s7Setup = json.Read().First();
			}
			catch (Exception ex)
			{
				log.Error(ex);
				s7Setup = new S7Setup
				{

					Name = "SIEMENS 08/15",
					IPAddress = "0.0.0.0",
					Rack = 0,
					Slot = 2
				};
			}
			return s7Setup;
		}

		private void LogData(object[] parameters)
		{
			log.Info("Logging all data blocks...");
			foreach (var d in parameters)
			{
				log.Info(d);
			}
		}

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
