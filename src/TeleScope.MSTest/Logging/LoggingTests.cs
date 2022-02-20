using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace TeleScope.MSTest.Logging
{
	[TestClass]
	public class LoggingTests : TestsBase
	{
		[TestInitialize]
		public override void Arrange()
		{
			base.ArrangeLogging<LoggingTests>();
		}

		[TestCleanup]
		public override void Cleanup()
		{

		}

		// -- test method

		[TestMethod]
		public void LogMessges_With_SourceName()
		{
			var log = LoggingProvider.CreateLogger("Log-Source-Name");

			// assert & act 
			AssertAndLog(log);
		}

		[TestMethod]
		public void LogMessges_With_Type()
		{
			// arrange
			var log = LoggingProvider.CreateLogger(typeof(LoggingTests));

			// assert & act 
			AssertAndLog(log);
		}

		[TestMethod]
		public void LogMessges_With_GenericType()
		{
			// arrange
			var log = LoggingProvider.CreateLogger<LoggingTests>();

			// assert & act 
			AssertAndLog(log);
		}

		[TestMethod]
		public void LogMetrics_With_NullPointer()
		{
			// arrange
			var level = LogLevel.Information;
			base.ArrangeLogging<LoggingTests>(level);
			var log = LoggingProvider.CreateLogger<LoggingTests>();

			// act 
			using var metric = log.Metrics();
			log.Info("Logging runs at level: {Level}", level);
			
			// assert
			Assert.IsNull(metric, "Metric should be null, but wasn`t.");
		}

		[TestMethod]
		public void LogMetrics_With_ThreeScopes()
		{
			// arrange
			var log = LoggingProvider.CreateLogger<LoggingTests>();

			// act & assert
			using var fullMetric = log.Metrics("Full scope metrics");
			Thread.Sleep(100);

			using (var innerMetric = log.Metrics(LogLevel.Trace, true, "Logging Metrics in definded scope in method {Method}", "LogMetrics")) 
			{
				using var _ = log.Metrics();		
				AssertAndLog(log);
			}
		}

		private void AssertAndLog(ILogger log)
		{
			// assert
			Assert.IsTrue(log != null, "The log was inactive");

			log.TraceMember();
			log.Trace("Tracing log");
			log.Debug("Debug log with params: {0} {1} {2}", 1, 2, 3);
			log.Info("Info log with source: {0}", this);
			log.Warn(new Exception("I`m warning you"), "Warning log with additional exception.", this);
			log.Error("Error log");
			log.Critical(new Exception("Now a really critical thing happended."));
		}
	}
}
