using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Json
{
	public class JsonProxy : IReadable, IWritable
	{

		// -- fields

		private ILogger _log;

		private JsonStorage _storage;

		// -- properties

		public bool CanCreate => _storage.CanCreate;

		public bool CanDelete => _storage.CanDelete;

		// -- constructors

		public JsonProxy(JsonStorage storage)
		{
			_log = LoggingProvider.CreateLogger<JsonProxy>();

			_storage = storage;
		}

		// -- methods;

		public  Read()
		{
			_storage.Read();
			_log.Trace("Some proxy code is running after reading...");
			return this;
		}

		public T As<T>()
		{
			T result = _storage.As<T>();
			_log.Trace("Some proxy code is running after converting data...");
			return result;
		}

		public void Write<T>(T data)
		{
			_log.Trace("Some proxy code is running before writing...");
			_storage.Write<T>(data);
		}
	}
}
