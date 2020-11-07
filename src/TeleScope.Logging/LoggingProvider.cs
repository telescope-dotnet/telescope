using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TeleScope.Logging
{
	public static class LoggingProvider
	{

		// -- properties

		public static ILoggerFactory Factory { get; private set; } = new NullLoggerFactory();

		/// <summary>
		/// Gets the fact wheather Factory is initialized of not. 
		/// </summary>
		public static bool IsInitialized => !Factory.GetType().IsAssignableFrom(typeof(NullLoggerFactory)); 

		// -- methods

		public static void Initialize(ILoggerFactory loggerFactory)
		{
			Factory = loggerFactory ?? new NullLoggerFactory();
		}

		public static void Clear()
		{
			Factory = new NullLoggerFactory();
		}
	}
}
