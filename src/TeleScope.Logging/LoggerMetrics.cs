using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleScope.Logging
{
	public class LoggerMetrics : IDisposable
	{
		// -- fields

		private readonly ILogger log;
		private readonly LogLevel logLevel;
		private readonly bool forceCollection;
		private readonly Stopwatch watch;
		private readonly string message;
		private readonly object[] args;


		//private bool isDisposed;

		// -- constructor

		public LoggerMetrics(ILogger logger, LogLevel logLevel, bool forceGarbageCollection, string message, params object[] args)
		{
			log = logger;
			this.logLevel = logLevel;
			forceCollection = forceGarbageCollection;
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
			log.Log(logLevel, $"{message} completed in {format(millis)} ms and total {format(memory)} KB memory in use.",  args);

			// -- local function

			static string format(Int64 number) 
			{
				return number.ToString("#,0.00", CultureInfo.CurrentCulture.NumberFormat);
			}
		}



		//protected virtual void Dispose(bool disposing)
		//{
		//	if (isDisposed)
		//	{
		//		return;

		//	}
		//	if (disposing)
		//	{
		//		// dispose managed resources
		//		watch?.Dispose();
		//		log?.Dispose();
		//	}

		//	// dispose unmanaged resources and finish
		//	isDisposed = true;
		//}
	}
}
