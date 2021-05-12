using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Compact;

namespace TeleScope.Logging.Extensions.Serilog
{
	/// <summary>
	/// This extension class provides methods simplify the configuration of Microsofts
	/// <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory">ILoggerFactory</see>.
	/// </summary>
	public static class ILoggerFactoryExtensions
	{
		// -- fields

		private static string template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}";
		private static LogLevel level = LogLevel.Information;
		private static ITextFormatter textFormatter = new CompactJsonFormatter();

		// -- extension methods

		/// <summary>
		/// Applies a template string to structure log messages.
		/// <example>
		/// The following sample shows how the default template is structured.
		/// <code>
		/// {Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="factory">The calling instance.</param>
		/// <param name="newTemplate">The new template string.</param>
		/// <returns>The calling instance.</returns>
		public static ILoggerFactory UseTemplate(this ILoggerFactory factory, string newTemplate)
		{
			template = newTemplate;
			return factory;
		}

		/// <summary>
		/// Applies a minimum log level.
		/// </summary>
		/// <param name="factory">The calling instance.</param>
		/// <param name="minimumLevel">The minumum log level as enum LogLevel.</param>
		/// <returns>The calling instance.</returns>
		public static ILoggerFactory UseLevel(this ILoggerFactory factory, LogLevel minimumLevel)
		{
			level = minimumLevel;
			return factory;
		}

		/// <summary>
		/// Applies a different text formatter for the logging of object data.
		/// <example>
		/// The following sample shows that the default formatter from Serilog is the `CompactJsonFormatter`.
		/// <code>
		/// private static ITextFormatter textFormatter = new CompactJsonFormatter();
		/// </code>
		/// </example>
		/// </summary>
		/// <param name="factory">The calling instance.</param>
		/// <param name="formatter"></param>
		/// <returns>The calling instance.</returns>
		public static ILoggerFactory UseFormatter(this ILoggerFactory factory, ITextFormatter formatter)
		{
			textFormatter = formatter;
			return factory;
		}

		/// <summary>
		/// Adds the console sink to the logger factory.
		/// </summary>
		/// <param name="factory">The calling instance.</param>
		/// <returns>The calling instance.</returns>
		public static ILoggerFactory AddSerilogConsole(this ILoggerFactory factory)
		{
			factory.AddSerilog(
				GetConfig()
					.WriteTo.Console(outputTemplate: template)
					.CreateLogger());
			return factory;
		}

		/// <summary>
		/// Adds the file sink to the logger factory.
		/// </summary>
		/// <param name="factory">The calling instance.</param>
		/// <param name="file">The file, where the log will be stored.</param>
		/// <returns>The calling instance.</returns>
		public static ILoggerFactory AddSerilogFile(this ILoggerFactory factory, string file)
		{
			factory.AddSerilog(
				GetConfig()
					.WriteTo.File(textFormatter, file)
					.CreateLogger());
			return factory;
		}

		// -- helper

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
