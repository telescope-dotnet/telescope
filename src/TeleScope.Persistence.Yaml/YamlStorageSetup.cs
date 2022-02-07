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

		public YamlStorageSetup(string file, WritePermissions permissions = WritePermissions.Create) : base(file, permissions)
		{

		}

		public YamlStorageSetup(FileInfo fileInfo, WritePermissions permissions = WritePermissions.Create) : base(fileInfo, permissions)
		{
			
		}
	}
}