using System;
using System.Collections.Generic;
using System.Threading;
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
			//base.Arrange();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- test method

		[TestMethod]
		public void TestStringSourceLog()
		{
			var log = LoggingProvider.CreateLogger("Log-Source-Name");

			// assert & act 
			AssertAndLog(log);
		}

		[TestMethod]
		public void TestTypedLog()
		{
			// arrange
			var log = LoggingProvider.CreateLogger(typeof(LoggingTests));

			// assert & act 
			AssertAndLog(log);
		}

		[TestMethod]
		public void TestGenericLog()
		{
			// arrange
			var log = LoggingProvider.CreateLogger<LoggingTests>();

			// assert & act 
			AssertAndLog(log);
		}

		[TestMethod]
		public void TestLokiSink()
		{
			var host = "https://devops.iwu.fraunhofer.de/loki/";

			LoggingProvider.Initialize(
				new LoggerFactory()
					.UseTemplate("{Timestamp: HH:mm:ss} [{Level} | {SourceContext:l}] - {Message}{NewLine}{Exception}")
					.UseLevel(LogLevel.Trace)
					.AddSerilogLoki(host, null, null, ("app", "TeleScope"), ("environment", "UnitTest")));

			var log = LoggingProvider.CreateLogger<LoggingTests>();

			// assert & act 
			Assert.IsTrue(log != null, "The log was inactive");
			var limit = 10;
			var i = 0;
			var sleep = 100;
			do
			{
				log.Trace("Tracing log");
				Thread.Sleep(sleep);
				log.Debug($"Debug log with params: {0} {1} {2}");
				log.Info("Info log at iteration: {0}", i);
				Thread.Sleep(sleep);
				log.Warn(new Exception("I`m waring you"), "Warning log with additional exception.");
				Thread.Sleep(sleep);
				log.Critical(new Exception("Now a really critical thing happended."));

				Console.WriteLine($"Iteration {i} done");
				Thread.Sleep(sleep);
				i++;
			} while (i < limit);
		}

		private void AssertAndLog(ILogger log)
		{
			Assert.IsTrue(log != null, "The log was inactive");
			log.Trace("Tracing log");
			log.Debug("Debug log with params: {0} {1} {2}", 1, 2, 3);
			log.Info("Info log with source: {0}", this);
			log.Warn(new Exception("I`m warning you"), "Warning log with additional exception.", this);
			log.Error("Error log");
			log.Critical(new Exception("Now a really critical thing happended."));
		}

		private void AssertAndLog<T>(ILogger<T> log)
		{
			// assert
			Assert.IsTrue(log != null, "The log was inactive");
			log.Trace("Tracing log");
			log.Debug("Debug log with params: {0} {1} {2}", 1, 2, 3);
			log.Info("Info log with source: {0}", this);
			log.Warn(new Exception("I`m warning you"), "Warning log with additional exception.", this);
			log.Error("Error log");
			log.Critical(new Exception("Now a really critical thing happended."));
		}
	}
}
