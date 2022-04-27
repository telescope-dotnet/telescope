using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using System.Globalization;
using TeleScope.Logging.Metrics.Abstractions;

namespace TeleScope.Logging.Metrics
{
	/// <summary>
	/// This static class provides new metric instances that will start immediately.
	/// </summary>
	public static class MetricProvider
	{
		// -- methods

		/// <summary>
		/// Creates a <see cref="NullMetric"/> instance that will not measure or log anything.
		/// </summary>
		/// <returns></returns>
		internal static IMetric Empty() 
		{
			return new NullMetric();
		}

		/// <summary>
		/// Starts new metrics with or without full memory collection and without internal logging.
		/// </summary>
		/// <returns>The new instance that is already measuring the metrics.</returns>
		public static IMetric StartNew(bool forceMemoryCollection = false) 
		{
			return new Metric(forceMemoryCollection);
		}

		/// <summary>
		/// Starts new metrics with a logger as information sink.
		/// </summary>
		/// <param name="forceMemoryCollection">Determines if the <see cref="GC"/> will perform a full memory collection or not.</param>
		/// <param name="logger">The logger sink.</param>
		/// <param name="logLevel">The minimum log level for logging the metrics.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">Optional data arguments that should appear in the message.</param>
		/// <returns>The new instance that is already measuring the metrics.</returns>
		public static IMetric StartNew(bool forceMemoryCollection, ILogger logger, LogLevel logLevel, string message, params object[] args)
		{
			return new Metric(forceMemoryCollection, logger, logLevel, message, args);
		}
	}
}