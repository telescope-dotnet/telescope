using System.IO;
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
		// -- properties

		/// <summary>
		/// Gets or sets the encoding of the file.
		/// </summary>
		public Encoding Encoder { get; set; }

		// -- constructor

		/// <summary>
		/// The default constructor calls the constructor of the base class and 
		/// defines `UTF8` as default <seealso cref="Encoder"/> property.
		/// </summary>
		/// <param name="fileInfo">The information about the file that will get accessed by a file storage.</param>
		/// <param name="canCreate">Sets the information, if the setup provides the ability to create files.</param>
		/// <param name="canDelete">Sets the information, if the setup provides the ability to delete files.</param>
		public JsonStorageSetup(
			FileInfo fileInfo,
			bool canCreate = true,
			bool canDelete = true) : base(fileInfo, canCreate, canDelete)
		{
			Encoder = Encoding.UTF8;
		}
	}
}