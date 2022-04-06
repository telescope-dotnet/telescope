using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics;
using System.Globalization;
using TeleScope.Logging.Metrics.Abstractions;

namespace TeleScope.Logging.Metrics
{
	internal sealed class Metric : IMetric
	{
		// -- fields

		private readonly Stopwatch watch;
		private readonly bool forceMemoryCollection;

		private readonly ILogger log;
		private readonly LogLevel logLevel;
		private readonly string message;
		private readonly object[] args;

		// -- properties

		public long EllapsedMilliseconds { get; private set; }

		public long AllocatedBytes { get; private set; }

		// -- constructor

		public Metric(bool forceMemoryCollection = false)
			: this(forceMemoryCollection, NullLogger.Instance, LogLevel.None, string.Empty)
		{

		}

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

		~Metric()
		{
			Dispose();
		}

		// -- methods

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Stop();

			log.Log(logLevel, $"Metrics: {message}", args);
			log.Log(logLevel, "Duration: {EllapsedMilliseconds}", FormatNumber(EllapsedMilliseconds));
			log.Log(logLevel, "Allocated Bytes: {AllocatedBytes}", FormatNumber(AllocatedBytes));
		}

		public void Stop()
		{
			watch.Stop();
			EllapsedMilliseconds = watch.ElapsedMilliseconds;
			AllocatedBytes = GC.GetTotalMemory(forceMemoryCollection);
		}

		// -- helper

		private static string FormatNumber(long number)
		{
			return number.ToString("#,0.00", CultureInfo.CurrentCulture.NumberFormat);
		}
	}
}
