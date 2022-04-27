using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using System.Globalization;
using TeleScope.Logging.Metrics.Abstractions;

namespace TeleScope.Logging.Metrics
{
	/// <summary>
	/// The sealed class implements the interface methods and provides default functionality to measure metrics and to log them.
	/// </summary>
	internal sealed class Metric : IMetric
	{
		// -- fields

		private const int MEGABYTE = 1048576;
		private const int KILOBYTE = 1024;

		private readonly Stopwatch watch;
		private readonly bool forceMemoryCollection;

		private readonly ILogger log;
		private readonly LogLevel logLevel;
		private readonly string message;
		private readonly object[] args;

		// -- properties

		/// <summary>
		/// Gets the ellapsed milliseconds between autostart and stop.
		/// </summary>
		public long EllapsedMilliseconds { get; private set; }

		/// <summary>
		/// Gets the allocated bytes between autostart and stop.
		/// </summary>
		public long AllocatedBytes { get; private set; }

		// -- constructor

		/// <summary>
		/// The default constructor starts a new metric measurement with or without a forced memory collection and
		/// without a logger.
		/// </summary>
		/// <param name="forceMemoryCollection">Determines if the <see cref="GC"/> will perform a full memory collection or not.</param>
		public Metric(bool forceMemoryCollection = false)
			: this(forceMemoryCollection, NullLogger.Instance, LogLevel.None, string.Empty)
		{

		}

		/// <summary>
		/// The default constructor starts a new metric measurement with or without a forced memory collection and
		/// with the specified logger data.
		/// </summary>
		/// <param name="forceMemoryCollection">Determines if the <see cref="GC"/> will perform a full memory collection or not.</param>
		/// <param name="logger">The logger as sink for log information.</param>
		/// <param name="logLevel">The minimum log level for logging the metrics.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">Optional data arguments that should appear in the message.</param>
		public Metric(bool forceMemoryCollection, ILogger logger, LogLevel logLevel, string message, params object[] args)
		{
			this.forceMemoryCollection = forceMemoryCollection;

			log = logger ?? NullLogger.Instance;
			this.logLevel = logLevel;
			this.message = message;
			this.args = args;

			watch = Stopwatch.StartNew();
		}

		// -- Finalizer

		/// <summary>
		/// Calls the <see cref="Dispose"/> method.
		/// </summary>
		~Metric()
		{
			Dispose();
		}

		// -- methods

		/// <summary>
		/// Calls the <see cref="Stop"/> method and logs the metrics, if a logger is present.
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Stop();

			log.Log(logLevel, $"Metrics: {message}", args);
			log.Log(logLevel, "Duration: {EllapsedMilliseconds}.", FormatDuration(EllapsedMilliseconds));
			log.Log(logLevel, "Allocated Memory: {AllocatedBytes}.", FormatBytes(AllocatedBytes));
		}

		/// <summary>
		/// Stops all measurements and provide the data through the properties.
		/// </summary>
		public void Stop()
		{
			watch.Stop();
			EllapsedMilliseconds = watch.ElapsedMilliseconds;
			AllocatedBytes = GC.GetTotalMemory(forceMemoryCollection);
		}

		// -- helper

		private static string FormatDuration(long number)
		{
			return $"{number.ToString("#,0.00", CultureInfo.CurrentCulture.NumberFormat)} ms";
		}

		private static string FormatBytes(long number)
		{
			float memory = number;
			string unit = "bytes";
			if (number > MEGABYTE)
			{
				memory = ((float)number / MEGABYTE);
				unit = "Mb";
			}
			else if (number > KILOBYTE)
			{
				memory = ((float)number / KILOBYTE);
				unit = "Kb";
			}
			return $"{memory.ToString("#,0.00", CultureInfo.CurrentCulture.NumberFormat)} {unit}";
		}
	}
}
