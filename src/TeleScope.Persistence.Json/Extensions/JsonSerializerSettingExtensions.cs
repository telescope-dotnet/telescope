using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TeleScope.Persistence.Json.Binder;

namespace TeleScope.Persistence.Json.Extensions
{
	/// <summary>
	/// The extension class enables a simplified binding of concrete types 
	/// to the `Newtonsoft.Json` de- and serialization process.
	/// </summary>
	public static class JsonSerializerSettingExtensions
	{
		/// <summary>
		/// Binds an enumerartion of `Type` to the calling instance of <seealso cref="JsonSerializerSettings"/>.
		/// </summary>
		/// <param name="settings">The calling instance.</param>
		/// <param name="types">The types that need to be known by the de- and serialization process.</param>
		public static void KnownTypes(this JsonSerializerSettings settings, IEnumerable<Type> types)
		{
			var binder = new KnownTypesBinder(types);
			settings.SerializationBinder = binder;
			settings.TypeNameHandling = TypeNameHandling.Auto;
		}
	}
}