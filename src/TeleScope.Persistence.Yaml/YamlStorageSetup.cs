using System.IO;
using System.Text;
using TeleScope.Persistence.Abstractions;
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
		/// <summary>
		/// Gets or sets the encoding of the file.
		/// </summary>
		public Encoding Encoder { get; set; }

		/// <summary>
		/// Gets or sets the value handing for default or null values.
		/// The default value is <see cref="DefaultValuesHandling.OmitNull"/>.
		/// </summary>
		public DefaultValuesHandling ValueHandling { get; set; }

		// -- constructor

		/// <summary>
		/// The default constructor calls the constructor of the base class and 
		/// defines `UTF8` as default <seealso cref="Encoder"/> property.
		/// </summary>
		/// <param name="fileInfo">The information about the file that will get accessed by a file storage.</param>
		/// <param name="canCreate">Sets the information, if the setup provides the ability to create files.</param>
		/// <param name="canDelete">Sets the information, if the setup provides the ability to delete files.</param>
		public YamlStorageSetup(
			FileInfo fileInfo,
			bool canCreate = true,
			bool canDelete = false) : base(fileInfo, canCreate, canDelete)
		{

			Encoder = Encoding.UTF8;
			ValueHandling = DefaultValuesHandling.OmitNull;
		}
	}
}