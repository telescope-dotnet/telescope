using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
		/// The internal <see cref="LogLevel"/> is set to <see cref="LogLevel.Trace"/>.
		/// The <see cref="GC"/> will not be forced to perform a full memory collection in order to optimize the execution time. 
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="memberName">The client-side message that contains the calling member name.
		/// If not present the message will only contain the name of the calling member <seealso cref="CallerMemberNameAttribute"/>.</param>
		/// <returns>The metrics instance as <see cref="IDisposable"/>.</returns>
		public static IDisposable Metrics(this ILogger logger, [CallerMemberName] string message = "")
		{
			return Metrics(logger, LogLevel.Trace, false, message);
		}

		public static IDisposable Metrics(this ILogger logger, LogLevel level, bool forceGarbageCollection, string message, params object[] args)
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
