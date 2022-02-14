using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using TeleScope.Connectors.Http.Abstractions;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace TeleScope.Connectors.Http.Caching
{
	public class HttpEndpointCache : ICacheable<string>
	{
		// -- fields

		private const int REFRESH_SECONDS = 3;
		private const int EXPIRATION_SECONDS = 9;

		private readonly ILogger log;
		private readonly MemoryCache cache;
		private readonly MemoryCacheEntryOptions options;

		// -- constructors

		public HttpEndpointCache(
			uint refreshSeconds = REFRESH_SECONDS,
			uint expirationSeconds = EXPIRATION_SECONDS)
			: this(TimeSpan.FromSeconds(refreshSeconds), TimeSpan.FromSeconds(expirationSeconds))
		{

		}

		public HttpEndpointCache(TimeSpan refreshExpiration, TimeSpan resetExpiration)
		{
			log = LoggingProvider.CreateLogger<HttpEndpointCache>();
			cache = new MemoryCache(new MemoryCacheOptions());
			options = new MemoryCacheEntryOptions()
				.SetSize(1024)
				.SetPriority(CacheItemPriority.High)
				.SetSlidingExpiration(refreshExpiration)
				.SetAbsoluteExpiration(resetExpiration);
		}

		// -- methods

		public string GetOrInvoke(string key, Func<string> invoke)
		{
			var success = cache.TryGetValue(key, out string item);
			if (!success)
			{
				item = invoke();
				Set(key, item);
			}
			else
			{
				log.Trace("Cache is present for {Key}", key);
			}
			return item;
		}

		// -- helper

		private void Set(string key, string item)
		{
			cache.Set(key, item, options);
		}
	}
}
