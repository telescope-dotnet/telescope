using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace TeleScope.Logging.Metrics.Extensions
{
	/// <summary>
	/// This extension class shortens log method calls.
	/// </summary>
	public static class ILoggerExtensions
	{
		// -- METRICS

		/// <summary>
		/// Writes the metrics `execution time` and `total memory usage` to the logger.
		/// The <see cref="LogLevel"/> is set to <see cref="LogLevel.Trace"/>.
		/// The <see cref="GC"/> will not be forced to perform a full memory collection in order to optimize the execution time. 
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="message">The client-side message that contains the calling member name.
		/// If not present the message will only contain the name of the calling member <seealso cref="CallerMemberNameAttribute"/>.</param>
		/// <returns>The metrics instance as <see cref="IDisposable"/>.</returns>
		public static IDisposable Metrics(this ILogger logger, [CallerMemberName] string message = "")
		{
			return Metrics(logger, LogLevel.Trace, false, message);
		}

		/// <summary>
		/// Writes the metrics `execution time` and `total memory usage` to the logger.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="level">The minimum log level for logging the metrics.</param>
		/// <param name="forceGarbageCollection">Determines if the <see cref="GC"/> will perform a full memory collection or not.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">Optional data arguments that should appear in the message.</param>
		/// <returns>The metrics instance as <see cref="IDisposable"/>.</returns>
		public static IDisposable Metrics(
			this ILogger logger,
			LogLevel level, 
			bool forceGarbageCollection,
			string message,
			params object[] args)
		{
			if (logger.IsEnabled(level))
			{
				return MetricProvider.StartNew(forceGarbageCollection, logger, level, message, args);
			}
			else
			{
				return MetricProvider.Empty();
			}
		}
	}
}
