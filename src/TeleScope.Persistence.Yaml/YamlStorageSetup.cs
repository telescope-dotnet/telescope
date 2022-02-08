using System.IO;
using System.Text;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;
using YamlDotNet.Serialization;

namespace TeleScope.Persistence.Yaml
{
	// -- properties

	/// <summary>
	/// This storage setup extends <seealso cref="FileSetupBase"/> 
	/// to specify parameters for an access to YAML files. 
	/// </summary>
	public class YamlStorageSetup : FileSetupBase
	{
		// -- properties

		/// <summary>
		/// Gets or sets the encoding of the file.
		/// The default value is <see cref=" Encoding.UTF8"/>.
		/// </summary>
		public Encoding Encoder { get; set; } = Encoding.UTF8;

		/// <summary>
		/// Gets or sets the value handing for default or null values.
		/// The default value is <see cref="DefaultValuesHandling.OmitNull"/>.
		/// </summary>
		public DefaultValuesHandling ValueHandling { get; set; } = DefaultValuesHandling.OmitNull;

		// -- constructors

		/// <summary>
		/// The constructor calls the according base class constructor and 
		/// leaves the default settings of the property.
		/// </summary>
		/// <param name="file">The file represented as string, the storage is attached to.</param>
		/// <param name="permissions">The permission flag to access the file.</param>
		public YamlStorageSetup(string file, WritePermissions permissions = WritePermissions.Create) 
			: base(file, permissions)
		{

		}

		/// <summary>
		/// The constructor calls the according base class constructor and 
		/// leaves the default settings of the property. 
		/// </summary>
		/// <param name="fileInfo">The file information, the storage is attached to.</param>
		/// <param name="permissions">The permission flag to access the file.</param>
		public YamlStorageSetup(FileInfo fileInfo, WritePermissions permissions = WritePermissions.Create) 
			: base(fileInfo, permissions)
		{
			
		}
	}
}