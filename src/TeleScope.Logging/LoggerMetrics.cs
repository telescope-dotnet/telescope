using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Globalization;

namespace TeleScope.Logging
{
	/// <summary>
	/// This class takes an <see cref="ILogger"/> in order to log performance metrics based on the scope where the instance exists.
	/// When the <see cref="Dispose"/> method is called the metrics `execution time` and `total memory` are measured and logged. 
	/// </summary>
	internal sealed class LoggerMetrics : IDisposable
	{
		// -- fields

		private readonly ILogger log;
		private readonly LogLevel logLevel;
		private readonly bool forceCollection;
		private readonly Stopwatch watch;
		private readonly string message;
		private readonly object[] args;

		// -- constructor

		/// <summary>
		/// The constructor takes all external objects to log client-side messages together with the metrics of its lifetime.
		/// </summary>
		/// <param name="logger">The logger instance that is used to log the metrics.</param>
		/// <param name="logLevel">The target <see cref="LogLevel"/>.</param>
		/// <param name="forceCollection">If true, the <see cref="GC"/> will be forced to perform a full collection.</param>
		/// <param name="message">The log message that will be put in front of the metrics.</param>
		/// <param name="args">The optional args that may be referenced in the log message.</param>
		public LoggerMetrics(ILogger logger, LogLevel logLevel, bool forceCollection, string message, params object[] args)
		{
			log = logger;
			this.logLevel = logLevel;
			this.forceCollection = forceCollection;
			this.message = message;
			this.args = args;
			watch = Stopwatch.StartNew();
		}

		// -- Finalizer

		~LoggerMetrics()
		{
			Dispose();
		}

		public void Dispose()
		{
			watch.Stop();
			var millis = watch.ElapsedMilliseconds;
			var memory = GC.GetTotalMemory(forceCollection) / 1024;
			var gcState = $"GC collection {(forceCollection ? "was" : "was NOT")} enforced";
			log.Log(logLevel, $"{message} - duration of scope: {format(millis)} ms - total memory: {format(memory)} KB ({gcState}).", args);

			GC.SuppressFinalize(this);

			// -- local function

			static string format(long number)
			{
				return number.ToString("#,0.00", CultureInfo.CurrentCulture.NumberFormat);
			}
		}
	}
}
