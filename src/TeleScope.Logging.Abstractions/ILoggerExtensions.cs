﻿using System;
using Microsoft.Extensions.Logging;

namespace TeleScope.Logging.Abstractions
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

		public static void Debug(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogDebug(ex, message, args);
		}
		public static void Debug(this ILogger logger, string message, params object[] args)
		{
			logger.LogDebug(message, args);
		}

		public static void Info(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogInformation(ex, message, args);
		}
		public static void Info(this ILogger logger, string message, params object[] args)
		{
			logger.LogInformation(message, args);
		}

		public static void Warn(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogWarning(ex, message, args);
		}
		public static void Warn(this ILogger logger, string message, params object[] args)
		{
			logger.LogWarning(message, args);
		}

		public static void Error(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogError(ex, message, args);
		}
		public static void Error(this ILogger logger, string message, params object[] args)
		{
			logger.LogError(message, args);
		}

		public static void Critical(this ILogger logger, Exception ex, string message, params object[] args)
		{
			logger.LogCritical(ex, message, args);
		}
		public static void Critical(this ILogger logger, string message, params object[] args)
		{
			logger.LogCritical(message, args);
		}
	}
}