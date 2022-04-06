using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using System.Globalization;
using TeleScope.Logging.Metrics.Abstractions;

namespace TeleScope.Logging.Metrics
{
	public static class MetricProvider
	{
		// -- methods

		internal static IMetric Empty() 
		{
			return new NullMetric();
		}

		/// <summary>
		/// Simple metrics with or without full memory collection and without internal logging.
		/// </summary>
		/// <returns></returns>
		public static IMetric StartNew(bool forceMemoryCollection = false) 
		{
			return new Metric(forceMemoryCollection);
		}

		/// <summary>
		/// Advanced metrics with or without full memory collection and with internal logging.
		/// </summary>
		/// <param name="forceMemoryCollection"></param>
		/// <param name="logger"></param>
		/// <param name="logLevel"></param>
		/// <returns></returns>
		public static IMetric StartNew(bool forceMemoryCollection, ILogger logger, LogLevel logLevel, string message, params object[] args)
		{
			return new Metric(forceMemoryCollection, logger, logLevel, message, args);
		}
	}
}