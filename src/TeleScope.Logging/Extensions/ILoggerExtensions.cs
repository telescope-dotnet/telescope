using System;
using Microsoft.Extensions.Logging;

namespace TeleScope.Logging.Extensions
{
	/// <summary>
	/// This extension class shortens log method calls.
	/// </summary>
	public static class ILoggerExtensions
	{
		// -- TRACE

		/// <summary>
		/// Writes a trace message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="ex">The exception that will to be logged.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Trace(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogTrace(ex, message, args);
		}
		/// <summary>
		/// Writes a trace message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Trace(this ILogger logger, string message, params object[] args)
		{
			logger.LogTrace(message, args);
		}

		/// <summary>
		/// Writes a trace message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Trace(this ILogger logger, object data)
		{
			logger.LogTrace(data.ToString());
		}

		// -- DEBUG

		/// <summary>
		/// Writes a debug message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="ex">The exception that will to be logged.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Debug(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogDebug(ex, message, args);
		}

		/// <summary>
		/// Writes a debug message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Debug(this ILogger logger, string message, params object[] args)
		{
			logger.LogDebug(message, args);
		}

		/// <summary>
		/// Writes a debug message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Debug(this ILogger logger, object data)
		{
			logger.LogDebug(data.ToString());
		}

		// -- INFO

		/// <summary>
		/// Writes a info message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="ex">The exception that will to be logged.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Info(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogInformation(ex, message, args);
		}

		/// <summary>
		/// Writes a info message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Info(this ILogger logger, string message, params object[] args)
		{
			logger.LogInformation(message, args);
		}

		/// <summary>
		/// Writes a info message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Info(this ILogger logger, object data)
		{
			logger.LogInformation(data.ToString());
		}

		// -- WARN

		/// <summary>
		/// Writes a warning message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="ex">The exception that will to be logged.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Warn(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogWarning(ex, message, args);
		}

		/// <summary>
		/// Writes a warning message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Warn(this ILogger logger, string message, params object[] args)
		{
			logger.LogWarning(message, args);
		}

		/// <summary>
		/// Writes a warning message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Warn(this ILogger logger, object data)
		{
			logger.LogWarning(data.ToString());
		}

		// -- ERROR

		/// <summary>
		/// Writes an error message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="ex">The exception that will to be logged.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Error(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogError(ex, message, args);
		}

		/// <summary>
		/// Writes an error message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Error(this ILogger logger, string message, params object[] args)
		{
			logger.LogError(message, args);
		}

		/// <summary>
		/// Writes an error message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Error(this ILogger logger, object data)
		{
			logger.LogError(data.ToString());
		}

		// -- CRITICAL

		/// <summary>
		/// Writes a critical message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="ex">The exception that will to be logged.</param>
		public static void Critical(this ILogger logger, Exception ex)
		{
			logger.LogCritical(ex, ex.Message);
		}

		/// <summary>
		/// Writes a critical message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="ex">The exception that will to be logged.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Critical(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogCritical(ex, message, args);
		}

		/// <summary>
		/// Writes a critical message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Critical(this ILogger logger, string message, params object[] args)
		{
			logger.LogCritical(message, args);
		}

		/// <summary>
		/// Writes a critical message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Critical(this ILogger logger, object data)
		{
			logger.LogCritical(data.ToString());
		}
	}
}
