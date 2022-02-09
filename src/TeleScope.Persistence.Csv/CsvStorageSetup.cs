using System.IO;
using System.Text;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;

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
		public char Separator { get; set; } = DEFAULT_SEPERATOR;

		/// <summary>
		/// Gets the index of the first row where the CSV data starts.
		/// </summary>
		public uint StartRow { get; set; } = DEFAULT_START_ROW;

		/// <summary>
		/// Gets a string that is used as heading line(s) before CSV data starts.
		/// </summary>
		public string Header { get; set; } = string.Empty;

		/// <summary>
		/// Gets the information if the CSV storage has header information or not.
		/// </summary>
		public bool HasHeader => !string.IsNullOrEmpty(Header);

		/// <summary>
		/// Gets or sets the encoding of the file.
		/// </summary>
		public Encoding Encoder { get; set; } = Encoding.UTF8;

		// -- constructors

		/// <summary>
		/// The constructor calls the according base class constructor and 
		/// leaves the default settings of the property.
		/// </summary>
		/// <param name="file">The file represented as string, the storage is attached to.</param>
		public CsvStorageSetup(string file) : base(file, WritePermissions.Create)
		{
		}

		/// <summary>
		/// The constructor calls the according base class constructor and 
		/// leaves the default settings of the property.
		/// </summary>
		/// <param name="fileInfo">The file information, the storage is attached to.</param>
		public CsvStorageSetup(FileInfo fileInfo) : base(fileInfo, WritePermissions.Create)
		{
		}
	}
}
