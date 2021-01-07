using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Factory;

namespace TeleScope.Persistence.Json
{
	public class JsonProxy : StorageProxyBase
	{

		// -- fields

		private ILogger _log;

		// -- constructors

		public JsonProxy(IStorable storage) : base(storage)
		{
			_log = LoggingProvider.CreateLogger<JsonProxy>();
		}

		// -- methods;

		public override T Read<T>()
		{
			T data = _storage.Read<T>();
			_log.Trace("Some proxy code is running after reading...");
			return data;
		}

		public override void Write<T>(T data)
		{
			_log.Trace("Some proxy code is running before writing...");
			_storage.Write<T>(data);
		}
	}
}
