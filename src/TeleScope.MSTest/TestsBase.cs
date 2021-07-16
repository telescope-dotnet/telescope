using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Logging.Extensions.Serilog;

namespace TeleScope.MSTest
{
	public abstract class TestsBase
	{

		// -- fields

		public const string APP_FOLDER = "App_Data";

		private Stopwatch watch;
		protected ILogger log;
		protected const string SKIP_PLC_TESTS = "SkipPlcTests";
		protected const string SKIP_SMTP_TESTS = "SkipSmtpTests";

		// -- base methods

		public virtual void Arrange()
		{
			watch = new Stopwatch();
			LoggingProvider.Initialize(
				 new LoggerFactory()
					 .UseTemplate("{Timestamp: HH:mm:ss} [{Level} | {SourceContext:l}] - {Message}{NewLine}{Exception}")
					 .UseLevel(LogLevel.Trace)
					 .AddSerilogConsole());
			log = LoggingProvider.CreateLogger<TestsBase>();
		}

		public virtual void Cleanup()
		{
			watch.Stop();
			watch = null;
		}

		protected long RunWithMetrics(string name, Action action)
		{
			watch.Restart();
			action();
			watch.Stop();
			var ms = watch.ElapsedMilliseconds;
			log.Trace($"'{name}' executed in {ms} ms.");

			return watch.ElapsedMilliseconds;
		}

		/// <summary>
		/// Helper method to throw specific exceptions and returns the properties
		/// </summary>
		/// <param name="context">The test context</param>
		/// <param name="key">The key to get the value of the property</param>
		/// <returns>The value of the property as a string.</returns>
		protected static string GetProperty(TestContext context, string key)
		{
			if (context.Properties[key] is null)
			{
				throw new ArgumentException($"The test property {key} does not exist, does not have a value, or a test setting is not selected");
			}
			else
			{
				return context.Properties[key].ToString();
			}
		}
	}
}