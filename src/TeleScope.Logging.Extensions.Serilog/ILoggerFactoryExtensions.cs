using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Loki;

namespace TeleScope.Logging.Extensions.Serilog
{
	public static class ILoggerFactoryExtensions
	{
		// -- fields

		private static string _template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}";
		private static LogLevel _level = LogLevel.Information;
		private static ITextFormatter _formatter = new CompactJsonFormatter();

		// -- extension methods

		public static ILoggerFactory UseTemplate(this ILoggerFactory factory, string newTemplate)
		{
			_template = newTemplate;
			return factory;
		}

		public static ILoggerFactory UseLevel(this ILoggerFactory factory, LogLevel minimumLevel)
		{
			_level = minimumLevel;
			return factory;
		}

		public static ILoggerFactory UseFormatter(this ILoggerFactory factory, ITextFormatter formatter)
		{
			_formatter = formatter;
			return factory;
		}

		public static ILoggerFactory AddSerilogConsole(this ILoggerFactory factory)
		{
			factory.AddSerilog(
				CreateConfig()
					.WriteTo.Console(outputTemplate: _template)
					.CreateLogger());
			return factory;
		}

		public static ILoggerFactory AddSerilogFile(this ILoggerFactory factory, string file)
		{
			factory.AddSerilog(
				CreateConfig()
					.WriteTo.File(_formatter, file)
					.CreateLogger());
			return factory;
		}

		public static ILoggerFactory AddSerilogLoki(this ILoggerFactory factory, string host, string user = null, string secret = null, params (string Key, string Value)[] labels)
		{
			var credentials = CreateCredentials(host, user, secret);
			var labelProvider = CreateLabels(labels);
			factory.AddSerilog(
				CreateConfig()
				.WriteTo.LokiHttp(credentials, labelProvider)
				.CreateLogger());

			return factory;

		}

		// -- helper methods

		private static LokiCredentials CreateCredentials(string host, string user = null, string secret = null)
		{
			if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(secret))
			{
				return new NoAuthCredentials(host);
			}
			else
			{
				return new BasicAuthCredentials(host, user, secret);
			}
		}

		private static LoggerConfiguration CreateConfig()
		{
			var config = new LoggerConfiguration();

			switch (_level)
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

		private static LogLabelProvider CreateLabels(params (string Key, string Value)[] labels)
		{
			return new LogLabelProvider(labels);
		}
	}
}
