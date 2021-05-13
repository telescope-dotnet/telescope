using System.IO;
using System.Text;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Csv
{
	/// <summary>
	/// This storage setup extends <seealso cref="FileSetupBase"/> 
	/// to specify parameters for an access to CSV files. 
	/// </summary>
	public class CsvStorageSetup : FileSetupBase
	{
		// --fields

		private const char DEFAULT_SEPERATOR = ';';
		private const uint DEFAULT_START_ROW = 0;

		// -- properties

		/// <summary>
		/// Gets the character to identify the separation between values.
		/// </summary>
		public char Separator { get; private set; }

		/// <summary>
		/// Gets the index of the first row where the CSV data starts.
		/// </summary>
		public uint StartRow { get; private set; }

		/// <summary>
		/// Gets a string that is used as heading line(s) before CSV data starts.
		/// </summary>
		public string Header { get; private set; }

		/// <summary>
		/// Gets the information if the CSV storage has header information or not.
		/// </summary>
		public bool HasHeader => !string.IsNullOrEmpty(Header);

		/// <summary>
		/// Gets or sets the encoding of the file.
		/// </summary>
		public Encoding Encoder { get; set; }

		// constructors

		/// <summary>
		/// The default constructor calls the constructor of the base class and 
		/// defines `UTF8` as default <seealso cref="Encoder"/> property.
		/// </summary>
		/// <param name="fileInfo">The information about the file that will get accessed by a file storage.</param>
		/// <param name="canCreate">Sets the information, if the setup provides the ability to create files.</param>
		/// <param name="canDelete">Sets the information, if the setup provides the ability to delete files.</param>
		/// <param name="separator">Sets the character to identify the separation between values.</param>
		/// <param name="startRow">Sets the index of the first row where the CSV data starts.</param>
		/// <param name="header">Sets a string that is used as heading line(s) before CSV data starts.</param>
		public CsvStorageSetup(
			FileInfo fileInfo,
			bool canCreate = DEFAULT_CAN_CREATE,
			bool canDelete = DEFAULT_CAN_DELETE,
			char separator = DEFAULT_SEPERATOR,
			uint startRow = DEFAULT_START_ROW,
			string header = default) : base(fileInfo, canCreate, canDelete)
		{
			Separator = separator;
			StartRow = startRow;
			Header = header;
			Encoder = Encoding.UTF8;
		}
	}
}
