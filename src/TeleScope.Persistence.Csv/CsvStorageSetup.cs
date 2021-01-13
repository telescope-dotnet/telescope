using System;
using System.IO;

namespace TeleScope.Persistence.Csv
{
	public class CsvStorageSetup
	{
		// --fields

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

		public bool CanCreate { get; set; }
		public bool CanDelete { get; set; }

		// constructor

		public CsvStorageSetup(FileInfo csvFileInfo, uint startRow = 0, char separator = ';', string header = default)
		{
			_csvFileInfo = csvFileInfo ?? throw new ArgumentNullException(nameof(csvFileInfo));
			StartRow = startRow;
			Separator = separator;
			Header = header;
			CanCreate = true;
			CanDelete = true;
		}
	}
}
