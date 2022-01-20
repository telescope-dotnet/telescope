using System;
using System.Runtime.CompilerServices;
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
		/// Writes a trace message with the calling member.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="memberName">The client-side message that contains the calling member name.</param>
		public static void TraceMember(this ILogger logger, [CallerMemberName] string memberName = "")
		{
			if (logger.IsEnabled(LogLevel.Trace))
				logger.LogTrace(memberName);
		}

		/// <summary>
		/// Writes a trace message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="ex">The exception that will to be logged.</param>
		/// <param name="message">The client-side message.</param>
		/// <param name="args">An array of objects that will to be logged.</param>
		public static void Trace(this ILogger logger, Exception ex, string message, params object[] args)
		{
			if (logger.IsEnabled(LogLevel.Trace))
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
			if (logger.IsEnabled(LogLevel.Trace))
				logger.LogTrace(message, args);
		}

		/// <summary>
		/// Writes a trace message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Trace(this ILogger logger, object data)
		{
			if (logger.IsEnabled(LogLevel.Trace))
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
			if (logger.IsEnabled(LogLevel.Debug)) 
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
			if (logger.IsEnabled(LogLevel.Debug))
				logger.LogDebug(message, args);
		}

		/// <summary>
		/// Writes a debug message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Debug(this ILogger logger, object data)
		{
			if (logger.IsEnabled(LogLevel.Debug))
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
			if (logger.IsEnabled(LogLevel.Information))
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
			if (logger.IsEnabled(LogLevel.Information))
				logger.LogInformation(message, args);
		}

		/// <summary>
		/// Writes a info message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Info(this ILogger logger, object data)
		{
			if (logger.IsEnabled(LogLevel.Information))
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
			if (logger.IsEnabled(LogLevel.Warning))
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
			if (logger.IsEnabled(LogLevel.Warning))
				logger.LogWarning(message, args);
		}

		/// <summary>
		/// Writes a warning message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Warn(this ILogger logger, object data)
		{
			if (logger.IsEnabled(LogLevel.Warning))
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
			if (logger.IsEnabled(LogLevel.Error))
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
			if (logger.IsEnabled(LogLevel.Error))
				logger.LogError(message, args);
		}

		/// <summary>
		/// Writes an error message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Error(this ILogger logger, object data)
		{
			if (logger.IsEnabled(LogLevel.Error))
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
			if (logger.IsEnabled(LogLevel.Critical))
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
			if (logger.IsEnabled(LogLevel.Critical))
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
			if (logger.IsEnabled(LogLevel.Critical))
				logger.LogCritical(message, args);
		}

		/// <summary>
		/// Writes a critical message.
		/// </summary>
		/// <param name="logger">The calling instance.</param>
		/// <param name="data">A data object whose string representation will be logged.</param>
		public static void Critical(this ILogger logger, object data)
		{
			if (logger.IsEnabled(LogLevel.Critical))
				logger.LogCritical(data.ToString());
		}
	}
}
