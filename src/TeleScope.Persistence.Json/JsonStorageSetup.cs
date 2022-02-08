using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;

namespace TeleScope.Persistence.Json
{
	/// <summary>
	/// This storage setup extends <seealso cref="FileSetupBase"/> 
	/// to specify parameters for an access to JSON files. 
	/// </summary>
	public class JsonStorageSetup : FileSetupBase
	{
		// -- properties

		/// <summary>
		/// Gets or sets the Newtonsoft JSON setting.
		/// </summary>
		public JsonSerializerSettings Settings { get; set; } = new JsonSerializerSettings();

		/// <summary>
		/// Gets or sets the formatting of the JSON file.
		/// The default value is <see cref="Formatting.Indented"/>.
		/// </summary>
		public Formatting Format { get; set; } = Formatting.Indented;

		/// <summary>
		/// Gets or sets the encoding of the file.
		/// </summary>
		public Encoding Encoder { get; set; } = Encoding.UTF8;

		// -- constructor

		/// <summary>
		/// The constructor calls the according base class constructor and 
		/// leaves the default settings of the property.
		/// </summary>
		/// <param name="file">The file represented as string, the storage is attached to.</param>
		public JsonStorageSetup(string file) : base(file, WritePermissions.Create)
		{

		}

		/// <summary>
		/// The default constructor calls the constructor of the base class and 
		/// defines `UTF8` as default <seealso cref="Encoder"/> property.
		/// </summary>
		/// <param name="fileInfo">The information about the file that will get accessed by a file storage.</param>
		public JsonStorageSetup(FileInfo fileInfo) : base(fileInfo, WritePermissions.Create)
		{

		}
	}
}