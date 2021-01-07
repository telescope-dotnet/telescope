using Newtonsoft.Json;
using TeleScope.Persistence.Abstractions.Factory;

namespace TeleScope.Persistence.Json.Extensions
{
	public static class JsonStorageExtensions
	{
		public static JsonStorage AddJson(this StorageFactory factory, string file, string key)
		{
			var json = new JsonStorage(file);
			factory.Add(key, json);
			return json;
		}

		public static JsonStorage AddJson(this StorageFactory factory, string file, string key, JsonSerializerSettings settings)
		{
			var json = new JsonStorage(file, settings);
			factory.Add(key, json);
			return json;
		}
	}
}
