using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Compact;

namespace TeleScope.Logging.Extensions.Serilog
{
	public static class ILoggerFactoryExtensions
	{
		// -- fields

		private static string template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}";
		private static LogLevel level = LogLevel.Information;
		private static ITextFormatter textFormatter = new CompactJsonFormatter();

		// -- extension methods

		public static ILoggerFactory UseTemplate(this ILoggerFactory factory, string newTemplate)
		{
			template = newTemplate;
			return factory;
		}

		public static ILoggerFactory UseLevel(this ILoggerFactory factory, LogLevel minimumLevel)
		{
			level = minimumLevel;
			return factory;
		}

		public static ILoggerFactory UseFormatter(this ILoggerFactory factory, ITextFormatter formatter)
		{
			textFormatter = formatter;
			return factory;
		}

		public static ILoggerFactory AddSerilogConsole(this ILoggerFactory factory)
		{
			factory.AddSerilog(
				GetConfig()
					.WriteTo.Console(outputTemplate: template)
					.CreateLogger());
			return factory;
		}

		public static ILoggerFactory AddSerilogFile(this ILoggerFactory factory, string file)
		{
			factory.AddSerilog(
				GetConfig()
					.WriteTo.File(textFormatter, file)
					.CreateLogger());
			return factory;
		}

		// -- helper methods

		private static LoggerConfiguration GetConfig()
		{
			var config = new LoggerConfiguration();

			switch (level)
			{
				case LogLevel.Critical:
					config.MinimumLevel.Fatal();
					break;
				case LogLevel.Error:
					config.MinimumLevel.Error();
					break;
				case LogLevel.Warning:
					config.MinimumLevel.Warning();
					break;
				case LogLevel.Information:
					config.MinimumLevel.Information();
					break;
				case LogLevel.Debug:
					config.MinimumLevel.Debug();
					break;
				case LogLevel.Trace:
					config.MinimumLevel.Verbose();
					break;
				default:
					config.MinimumLevel.Information();
					break;
			}
			return config;
		}
	}
}
