using System.Text;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Csv
{
	public class CsvStorageSetup : FileSetupBase
	{
		// --fields

		private const char DEFAULT_SEPERATOR = ';';
		private const uint DEFAULT_START_ROW = 0;

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

		public Encoding Encoder { get; set; }

		// constructors

		public CsvStorageSetup(
			string file,
			bool canCreate = DEFAULT_CAN_CREATE,
			bool canDelete = DEFAULT_CAN_DELETE,
			char separator = DEFAULT_SEPERATOR,
			uint startRow = DEFAULT_START_ROW,
			string header = default) : base(file, canCreate, canDelete)
		{
			Separator = separator;
			StartRow = startRow;
			Header = header;
			Encoder = Encoding.UTF8;
		}
	}
}
