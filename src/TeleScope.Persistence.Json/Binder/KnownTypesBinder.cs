using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace TeleScope.Persistence.Json.Binder
{
	/// <summary>
	/// This class collects <seealso cref="Type"/> objects in order to include their names intor the 
	/// de- and serialization process of Newtonsoft.Json.
	/// For more information, see [custom SerializationBinder](https://www.newtonsoft.com/json/help/html/SerializeSerializationBinder.htm).
	/// </summary>
	public class KnownTypesBinder : ISerializationBinder
	{
		// -- fields

		private readonly IEnumerable<Type> knownTypes;

		// -- constructor

		/// <summary>
		/// The default constructor takes all known types as input parameter stores them internally.
		/// </summary>
		/// <param name="types">The enumeration of known types.</param>
		public KnownTypesBinder(IEnumerable<Type> types)
		{
			knownTypes = types;
		}

		// -- methods

		/// <summary>
		/// Controls the binding of a serialized object to a type.
		/// See [ISerializationBinder](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Serialization_ISerializationBinder.htm).
		/// </summary>
		/// <param name="assemblyName">Specifies the Assembly name of the serialized object.</param>
		/// <param name="typeName">Specifies the Type name of the serialized object.</param>
		/// <returns></returns>
		public Type BindToType(string assemblyName, string typeName)
		{
			return knownTypes.SingleOrDefault(t => t.Name == typeName);
		}

		/// <summary>
		/// Controls the binding of a serialized object to a type.
		/// See [ISerializationBinder](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Serialization_ISerializationBinder.htm).
		/// </summary>
		/// <param name="serializedType">The type of the object the formatter creates a new instance of.</param>
		/// <param name="assemblyName">Specifies the Assembly name of the serialized object.</param>
		/// <param name="typeName">Specifies the Type name of the serialized object.</param>
		public void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = null;
			typeName = serializedType.Name;
		}
	}
}
