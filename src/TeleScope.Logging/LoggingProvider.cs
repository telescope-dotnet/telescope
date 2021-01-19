using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TeleScope.Logging
{
	public static class LoggingProvider
	{

		// -- properties

		private static ILoggerFactory _factory = new NullLoggerFactory();

		// -- methods

		public static void Initialize(ILoggerFactory loggerFactory)
		{
			_factory = loggerFactory ?? new NullLoggerFactory();
		}

		public static ILogger CreateLogger(string name)
		{
			return _factory.CreateLogger(name);
		}

		public static ILogger CreateLogger(Type type)
		{
			return _factory.CreateLogger(type);
		}

		public static ILogger<T> CreateLogger<T>()
		{
			return _factory.CreateLogger<T>();
		}
	}
}
