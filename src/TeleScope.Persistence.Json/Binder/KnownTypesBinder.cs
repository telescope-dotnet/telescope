using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace TeleScope.Persistence.Json.Binder
{
	public class KnownTypesBinder : ISerializationBinder
	{
		// -- properties

		public IEnumerable<Type> KnownTypes { get; set; }

		// -- constructor

		public KnownTypesBinder()
		{
			KnownTypes = new List<Type>();

		}

		public KnownTypesBinder(IEnumerable<Type> types)
		{
			KnownTypes = types;
		}

		// -- methods

		public Type BindToType(string assemblyName, string typeName)
		{
			return KnownTypes.SingleOrDefault(t => t.Name == typeName);
		}

		public void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = null;
			typeName = serializedType.Name;
		}
	}
}
