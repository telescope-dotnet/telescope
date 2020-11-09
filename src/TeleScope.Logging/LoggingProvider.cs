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

		public static ILogger CreateLogger<T>()
		{
			return _factory.CreateLogger<T>();
		}
	}
}
