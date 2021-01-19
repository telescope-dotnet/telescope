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
			base.Arrange();
			LoggingProvider.Initialize(
				new LoggerFactory()
					.UseLevel(LogLevel.Trace)
					.AddSerilogConsole());
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
			var genericLog = LoggingProvider.CreateLogger<LoggingTests>();
			var typedLog = LoggingProvider.CreateLogger(this.GetType());
			var namedLog = LoggingProvider.CreateLogger("GivenSource");


			// act
			genericLog.Trace("Tracing log");
			genericLog.Debug("Debug log with params: {0} {1} {2}", 1, 2, 3);

			typedLog.Info("Info log with source: {0}", this);
			typedLog.Warn(new Exception("I`m warning you"), "Warning log with additional exception.", this);
			
			namedLog.Error("Error log");
			namedLog.Critical(new Exception("Now a really critical thing happended."));

			// assert
			Assert.IsTrue(_log != null, "The log was inactive");
		}
	}
}
