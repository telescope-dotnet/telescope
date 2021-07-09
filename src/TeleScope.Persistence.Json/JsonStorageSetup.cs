using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Json
{
	/// <summary>
	/// This storage setup extends <seealso cref="FileSetupBase"/> 
	/// to specify parameters for an access to JSON files. 
	/// </summary>
	public class JsonStorageSetup : FileSetupBase
	{
		// -- fields

		private readonly List<Type> typeConversions;

		// -- properties

		/// <summary>
		/// Gets or sets the encoding of the file.
		/// </summary>
		public Encoding Encoder { get; set; }

		/// <summary>
		/// Gets or sets the behaviour to convert enums automatically into strings. The default value is `True`.
		/// </summary>
		public bool ConvertEnumToString { get; set; }

		/// <summary>
		/// Gets the defined types that are used to convet base classes or interfaces into concrete types.
		/// </summary>
		public IEnumerable<Type> TypeConversions { get; }

		// -- constructor

		/// <summary>
		/// The default constructor calls the constructor of the base class and 
		/// defines `UTF8` as default <seealso cref="Encoder"/> property.
		/// <param name="fileInfo">The information about the file that will get accessed by a file storage.</param>
		/// <param name="canCreate">Sets the information, if the setup provides the ability to create files.</param>
		/// <param name="canDelete">Sets the information, if the setup provides the ability to delete files.</param>
		/// <param name="typeConversions"></param>
		public JsonStorageSetup(
			FileInfo fileInfo,
			bool canCreate = true,
			bool canDelete = true, IEnumerable<Type> typeConversions = null) : base(fileInfo, canCreate, canDelete)
		{
			Encoder = Encoding.UTF8;
			ConvertEnumToString = true;
			this.typeConversions = typeConversions?.ToList() ?? new List<Type>();
		}
	}
}