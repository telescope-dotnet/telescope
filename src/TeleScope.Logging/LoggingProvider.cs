using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TeleScope.Logging
{
	/// <summary>
	/// The static class provides an easy-to-use API to inject the <seealso cref="ILoggerFactory"/>
	/// and to use provided loggers on client-side code.
	/// </summary>
	public static class LoggingProvider
	{

		// -- fields

		private static ILoggerFactory factory = new NullLoggerFactory();

		// -- methods

		/// <summary>
		/// Stores the given logger factory that provides loggers via the provided `CreateLogger` methods.
		/// If the parameter is null the <seealso cref="NullLoggerFactory"/> is used internally.
		/// </summary>
		/// <param name="loggerFactory">The logger factory that will provide concrete loggers.</param>
		public static void Initialize(ILoggerFactory loggerFactory)
		{
			factory = loggerFactory ?? new NullLoggerFactory();
		}

		/// <summary>
		/// Get a logger from the internal logger factory and use a name to tag the source.
		/// </summary>
		/// <param name="name">The name tags the source of the log messages.</param>
		/// <returns>The concrete logger.</returns>
		public static ILogger CreateLogger(string name)
		{
			return factory.CreateLogger(name);
		}

		/// <summary>
		/// Get a logger from the internal logger factory and use a <seealso cref="Type"/> instance to tag the source.
		/// </summary>
		/// <param name="type">The type tags the source of the log messages.</param>
		/// <returns>The concrete logger.</returns>
		public static ILogger CreateLogger(Type type)
		{
			return factory.CreateLogger(type);
		}

		/// <summary>
		/// Get a logger from the internal logger factory and use a generic type to tag the source.
		/// </summary>
		/// <typeparam name="T">The generic type T tags the source of the log messages.</typeparam>
		/// <returns>The concrete logger.</returns>
		public static ILogger<T> CreateLogger<T>()
		{
			return factory.CreateLogger<T>();
		}
	}
}
