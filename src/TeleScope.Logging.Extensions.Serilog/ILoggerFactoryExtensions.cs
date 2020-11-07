using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace TeleScope.Logging.Extensions.Serilog
{
	public static class ILoggerFactoryExtensions
	{
		private static string _template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}";

		public static ILoggerFactory UseTemplate(this ILoggerFactory factory, string newTemplate)
		{
			_template = newTemplate;
			return factory;
		}

		public static ILoggerFactory AddSerilog(this ILoggerFactory factory, LogLevel minimumLevel)
		{
			factory.AddSerilog(
				GetConfig(minimumLevel)
					.CreateLogger());
			return factory;
		}

		public static ILoggerFactory AddSerilog(this ILoggerFactory factory, string file)
		{
			factory.AddSerilog(
				GetConfig(LogLevel.Information)
					.WriteTo.File(new CompactJsonFormatter(), file)
					.CreateLogger());
			return factory;
		}

		public static ILoggerFactory AddSerilog(this ILoggerFactory factory, LogLevel minimumLevel, string file)
		{
			factory.AddSerilog(
				GetConfig(minimumLevel)
					.WriteTo.File(new CompactJsonFormatter(), file)
					.CreateLogger());
			return factory;
		}

		// -- helper methods

		private static LoggerConfiguration GetConfig(LogLevel minimumLevel)
		{
			var config = new LoggerConfiguration()
				.WriteTo.Console(outputTemplate: _template);

			switch (minimumLevel)
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
			};
			return config;
		}
	}
}
