using System;
using Microsoft.Extensions.Logging;

namespace TeleScope.Logging.Extensions
{
	public static class ILoggerExtensions
	{
		public static void Trace(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogTrace(ex, message, args);
		}
		public static void Trace(this ILogger logger, string message, params object[] args)
		{
			logger.LogTrace(message, args);
		}
		public static void Trace(this ILogger logger, object data)
		{
			logger.LogTrace(data.ToString());
		}

		public static void Debug(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogDebug(ex, message, args);
		}
		public static void Debug(this ILogger logger, string message, params object[] args)
		{
			logger.LogDebug(message, args);
		}
		public static void Debug(this ILogger logger, object data)
		{
			logger.LogDebug(data.ToString());
		}

		public static void Info(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogInformation(ex, message, args);
		}
		public static void Info(this ILogger logger, string message, params object[] args)
		{
			logger.LogInformation(message, args);
		}
		public static void Info(this ILogger logger, object data)
		{
			logger.LogInformation(data.ToString());
		}

		public static void Warn(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogWarning(ex, message, args);
		}
		public static void Warn(this ILogger logger, string message, params object[] args)
		{
			logger.LogWarning(message, args);
		}
		public static void Warn(this ILogger logger, object data)
		{
			logger.LogWarning(data.ToString());
		}

		public static void Error(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogError(ex, message, args);
		}
		public static void Error(this ILogger logger, string message, params object[] args)
		{
			logger.LogError(message, args);
		}
		public static void Error(this ILogger logger, object data)
		{
			logger.LogError(data.ToString());
		}

		public static void Critical(this ILogger logger, Exception ex)
		{
			logger.LogCritical(ex, ex.Message);
		}
		public static void Critical(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogCritical(ex, message, args);
		}
		public static void Critical(this ILogger logger, string message, params object[] args)
		{
			logger.LogCritical(message, args);
		}
		public static void Critical(this ILogger logger, object data)
		{
			logger.LogCritical(data.ToString());
		}
	}
}
