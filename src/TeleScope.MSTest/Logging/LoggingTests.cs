using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using TeleScope.Connectors.Abstractions;
using TeleScope.Connectors.Plc.Siemens;
using TeleScope.Logging;
using TeleScope.Logging.Extensions.Serilog;
using TeleScope.Logging.Extensions;
using System;

namespace TeleScope.MSTest.Logging
{
	[TestClass]
	public class LoggingTests : TestsBase
	{
		[TestInitialize]
		public override void Arrange()
		{
			// old version
			/*
			var loggerFactory = (ILoggerFactory)new LoggerFactory();
			var serilogLogger =
				new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}")
				.WriteTo.File(new CompactJsonFormatter(), "./App_Data//log.json")
				.CreateLogger();
			loggerFactory.AddSerilog(serilogLogger);
			LoggingProvider.Initialize(loggerFactory);
			*/

			// new version: TeleScope.Logging.Extensions.Serilog;
			LoggingProvider.Initialize(
				new LoggerFactory()
					.UseTemplate("{Timestamp: HH:mm:ss} [{Level}] - {Message}{NewLine}{Exception}")
					.AddSerilog(LogLevel.Trace, "./App_Data/log/log.json"));
			base.Arrange();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- test method

		[TestMethod]
		public void TestSerilog()
		{
			// arrange
			var _log = LoggingProvider.Factory.CreateLogger<LoggingTests>();

			// act
			_log.Trace("Tracing log");
			_log.Debug("Debug log with params: {0} {1} {2}", 1, 2, 3);
			_log.Info("Info log with source: {0}", this);
			_log.Warn(new Exception("I`m waring you"), "Warning log with additional exception.", this);
			_log.Error("Error log");
			_log.Critical(new Exception("Now a really critical thing happended."));

			// assert
			Assert.IsTrue(LoggingProvider.IsInitialized, "The log was inactive");
		}

		[TestMethod]
		public void TestS7ConnectorLogging()
		{
			// arrange
			var connector = new S7Connector();

			// act
			connector.Setup(new MockupSetup());
			connector.Setup(new S7Setup());

			// assert
			Assert.IsTrue(LoggingProvider.IsInitialized, "The log was inactive");
		}
	}

	class MockupSetup : SetupBase
	{

	}
}
