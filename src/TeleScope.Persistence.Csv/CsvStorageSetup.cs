using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TeleScope.Persistence.Csv
{
	public class CsvStorageSetup
	{

        // --fields

        private FileInfo _info;

        // -- properties

        /// <summary>
        /// Gibt das Trennzeichen zwischen den Spalten aus oder legt dieses fest.
        /// </summary>
        public char Separator { get; set; }

        public int StartIndex { get; set; }

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
