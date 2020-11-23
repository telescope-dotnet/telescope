using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Crude;

namespace TeleScope.Persistence.Json.Extensions
{
	public static class JsonCrudeExtensions
	{
		public static JsonCrude AddJson(this CrudeFactory factory, string file)
		{
			var json = new JsonCrude(file);
			factory.Set(json);
			return json;
		}

		public static JsonCrude AddJson(this CrudeFactory factory, string file, JsonSerializerSettings settings)
		{
			var json = new JsonCrude(file, settings);
			factory.Set(json);
			return json;
		}
	}
}
