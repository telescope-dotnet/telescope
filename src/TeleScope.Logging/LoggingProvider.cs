using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TeleScope.Logging
{
	public static class LoggingProvider
	{

		// -- properties

		private static ILoggerFactory factory = new NullLoggerFactory();

		// -- methods

		public static void Initialize(ILoggerFactory loggerFactory)
		{
			factory = loggerFactory ?? new NullLoggerFactory();
		}

		public static ILogger CreateLogger(string name)
		{
			return factory.CreateLogger(name);
		}

		public static ILogger CreateLogger(Type type)
		{
			return factory.CreateLogger(type);
		}

		public static ILogger<T> CreateLogger<T>()
		{
			return factory.CreateLogger<T>();
		}
	}
}
