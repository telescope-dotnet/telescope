using System;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Compact;

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
				GetConfig()
					.WriteTo.Console(outputTemplate: _template)
					.CreateLogger());
			return factory;
		}

		public static ILoggerFactory AddSerilogFile(this ILoggerFactory factory, string file)
		{
			factory.AddSerilog(
				GetConfig()
					.WriteTo.File(_formatter, file)
					.CreateLogger());
			return factory;
		}

		/// <summary>
		/// Not implemented yet
		/// </summary>
		/// <param name="factory"></param>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static ILoggerFactory AddSerilogHttp(this ILoggerFactory factory, Uri uri)
		{
			throw new NotImplementedException();
		}

		// -- helper methods

		private static LoggerConfiguration GetConfig()
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
			};
			return config;
		}
	}
}
