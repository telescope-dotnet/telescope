using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TeleScope.Persistence.Json.Binder;

namespace TeleScope.Persistence.Json.Extensions
{
	public static class JsonSerializerSettingExtensions
	{
		public static void KnownTypes(this JsonSerializerSettings settings, IEnumerable<Type> types)
		{
			var binder = new KnownTypesBinder(types);
			settings.SerializationBinder = binder;
			settings.TypeNameHandling = TypeNameHandling.Auto;
		}
	}
}