using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Logging.Extensions.Serilog;

namespace TeleScope.MSTest.Logging
{
	[TestClass]
	public class LoggingTests : TestsBase
	{
		[TestInitialize]
		public override void Arrange()
		{
			LoggingProvider.Initialize(
				new LoggerFactory()
					.UseTemplate("{Timestamp: HH:mm:ss} [{Level}] - {Message}{NewLine}{Exception}")
					.UseLevel(LogLevel.Trace)
					.AddSerilogConsole());
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
			var _log = LoggingProvider.CreateLogger<LoggingTests>();

			// act
			_log.Trace("Tracing log");
			_log.Debug("Debug log with params: {0} {1} {2}", 1, 2, 3);
			_log.Info("Info log with source: {0}", this);
			_log.Warn(new Exception("I`m waring you"), "Warning log with additional exception.", this);
			_log.Error("Error log");
			_log.Critical(new Exception("Now a really critical thing happended."));

			// assert
			Assert.IsTrue(_log != null, "The log was inactive");
		}
	}
}
