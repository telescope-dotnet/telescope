using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using TeleScope.Connectors.Http.Abstractions;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace TeleScope.Connectors.Http.Caching
{
	/// <summary>
	/// This class provides a method to invoke external functions and cache its <see cref="string"/> results.
	/// </summary>
	public class StringMemoryCache : ICacheable<string>
	{
		// -- fields
		
		private const int REFRESH_SECONDS = 3;
		private const int EXPIRATION_SECONDS = 9;

		private readonly ILogger log;
		private readonly MemoryCache cache;
		private readonly MemoryCacheEntryOptions options;

		private bool isDisposed;

		// -- constructors

		/// <summary>
		/// The constructor instanciates the internal caching with settings, 
		/// based on the optional parameters or with default settings. 
		/// </summary>
		/// <param name="refreshSeconds">The timeout in seconds where the cache will return (refresh) the cached data.</param>
		/// <param name="expirationSeconds">The timeout in seconds where the cache will expire the cached data.</param>
		public StringMemoryCache(
			uint refreshSeconds = REFRESH_SECONDS,
			uint expirationSeconds = EXPIRATION_SECONDS)
			: this(TimeSpan.FromSeconds(refreshSeconds), TimeSpan.FromSeconds(expirationSeconds))
		{

		}

		/// <summary>
		/// The constructor instanciates the internal caching with settings, 
		/// based on the two <see cref="TimeSpan"/> parameters.
		/// </summary>
		/// <param name="refreshExpiration">The timeout where the cache will return (refresh) the cached data.</param>
		/// <param name="resetExpiration">The timeout where the cache will expire the cached data.</param>
		public StringMemoryCache(TimeSpan refreshExpiration, TimeSpan resetExpiration)
		{
			log = LoggingProvider.CreateLogger<StringMemoryCache>();
			cache = new MemoryCache(new MemoryCacheOptions());
			options = new MemoryCacheEntryOptions()
				.SetSize(1024)
				.SetPriority(CacheItemPriority.High)
				.SetSlidingExpiration(refreshExpiration)
				.SetAbsoluteExpiration(resetExpiration);
		}

		/// <summary>
		/// The finalizer disposes the unmanged resources. 
		/// </summary>
		~StringMemoryCache()
		{
			Dispose(false);
		}

		// -- methods

		/// <summary>
		/// Disposes all managed resources and supresses the <see cref="GC"/> to call the finalizer.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Implements the Dispose function. 
		/// </summary>
		/// <param name="disposing">If True, the internal managed resouces will be disposed.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed)
			{
				return;

			}
			if (disposing)
			{
				// dispose managed resources
				cache?.Dispose();
			}

			// dispose unmanaged resources and finish
			isDisposed = true;
		}

		/// <summary>
		/// Returs the cached data, if the key holds it already or invokes the external function.
		/// </summary>
		/// <param name="key">The key that is used to store the result of the external function call.</param>
		/// <param name="invoke">The external function that shall be called if the cache contains no data.</param>
		/// <returns>The result of type T from cache or the external function call.</returns>
		public string GetOrInvoke(string key, Func<string> invoke)
		{
			var success = cache.TryGetValue(key, out string item);
			if (!success)
			{
				item = invoke();
				SetCache(key, item);
			}
			else
			{
				log.Trace("Cache is present for {Key}", key);
			}
			return item;
		}

		// -- helper

		/// <summary>
		/// Sets the new result into the cache with the given key.
		/// </summary>
		/// <param name="key">The key that is associated with the cached data.</param>
		/// <param name="data">The data that will be cached.</param>
		private void SetCache(string key, string data)
		{
			cache.Set(key, data, options);
		}
	}
}
