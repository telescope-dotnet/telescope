using System.IO;

namespace TeleScope.Persistence.Csv
{
	public class CsvStorageSetup
	{

		// --fields

		private readonly FileInfo _info;

		// -- properties

		/// <summary>
		/// Gets or sets the character to identify the separation between values.
		/// </summary>
		public char Separator { get; set; }

		/// <summary>
		/// Gets or sets the index of the first row where the character seperated values are beginning.
		/// </summary>
		public int StartRow { get; set; }

		public string Header { get; set; }

		public bool HasHeader => !string.IsNullOrEmpty(Header);

		public string File => _info.FullName;

		public string Filename => _info.Name;

		public string Extension => _info.Extension;

		public string Location => _info.Directory.FullName;

		// constructor

		public CsvStorageSetup(string file)
		{
			Separator = ';';
			_info = new FileInfo(file);
		}
	}
}
