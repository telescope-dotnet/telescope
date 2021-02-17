using System;
using System.IO;
using System.Text;

namespace TeleScope.Persistence.Csv
{
	public class CsvStorageSetup
	{
		// --fields

		private const char DEFAULT_SEPERATOR = ';';
		private const uint DEFAULT_START_ROW = 0;
		private const bool DEFAULT_CAN_CREATE = true;
		private const bool DEFAULT_CAN_DELETE = true;

		private readonly FileInfo _csvFileInfo;

		// -- properties

		/// <summary>
		/// Gets or sets the character to identify the separation between values.
		/// </summary>
		public char Separator { get; private set; }

		/// <summary>
		/// Gets or sets the index of the first row where the character seperated values are beginning.
		/// </summary>
		public uint StartRow { get; private set; }

		public string Header { get; private set; }

		public bool HasHeader => !string.IsNullOrEmpty(Header);

		public string File => _csvFileInfo.FullName;

		public string Filename => _csvFileInfo.Name;

		public string Extension => _csvFileInfo.Extension;

		public string Location => _csvFileInfo.Directory.FullName;

		public Encoding Encoder { get; set; }

		public bool CanCreate { get; set; }

		public bool CanDelete { get; set; }

		// constructors

		public CsvStorageSetup(
			FileInfo csvFileInfo,
			uint startRow = DEFAULT_START_ROW,
			char separator = DEFAULT_SEPERATOR,
			string header = default,
			bool canCreate = DEFAULT_CAN_CREATE,
			bool canDelete = DEFAULT_CAN_DELETE)
		{
			_csvFileInfo = csvFileInfo ?? throw new ArgumentNullException(nameof(csvFileInfo));
			StartRow = startRow;
			Separator = separator;
			Header = header;
			CanCreate = canCreate;
			CanDelete = canDelete;
			Encoder = Encoding.Unicode;
		}
	}
}
