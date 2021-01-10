using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Csv
{
	public class CsvStorage<T> : IReadable<string[], T>, IWritable<T, string[]>
	{

		// -- fields

		private ILogger _log;
		private CsvStorageSetup _setup;

		// -- properties

		public bool CanCreate { get; private set; }
		public bool CanDelete { get; private set; }
		public IParsable<T> IncomingParser { get; set; }
		public IParsable<string[]> OutgoingParser { get; set; }

		// -- concstructor

		public CsvStorage(CsvStorageSetup setup, bool canCreate = true, bool canDelete = true)
		{
			_log = LoggingProvider.CreateLogger<CsvStorage<T>>();
			_setup = setup;

			// TODO implement create and delete behavior
			CanCreate = canCreate;
			CanDelete = canDelete;
		}

		public IEnumerable<T> Read()
		{
			List<T> result = new List<T>();

			string[] lines = File.ReadAllLines(_setup.File);

			// read all rows
			for (int i = _setup.StartRow; i < lines.Length; i++)
			{
				string[] fields = lines[i].Split(_setup.Separator);
				result.Add(IncomingParser.Parse<string[]>(fields));
			}

			_log.Trace($"csv import successfull for '{_setup.Filename}'");

			return result;
		}

		public void Write(IEnumerable<T> data)
		{

			if (data == null)
			{
				if (CanDelete)
				{
					Delete();
				}
				return;
			}

			if (CanCreate &&
				!string.IsNullOrEmpty(_setup.Location) &&
				!Directory.Exists(_setup.Location))
			{
				Create(_setup.Location);
			}

			var csv = new StringBuilder();
			var result = string.Empty;
			var seperator = _setup.Separator.ToString();

			if (_setup.HasHeader)
			{
				csv.AppendLine(_setup.Header);
			}

			// append data
			foreach (T item in data)
			{
				var line = string.Join(seperator, OutgoingParser.Parse<T>(item));
				csv.AppendLine(line);
			}

			// flush to file
			File.WriteAllText(_setup.File, csv.ToString());
			_log.Trace($"csv export successfull for '{_setup.Filename}'");
		}

		private void Create(string location)
		{
			Directory.CreateDirectory(location);
			_log.Trace("Directory created for file: {0}", _setup.File);
		}

		private void Delete()
		{
			if (File.Exists(_setup.File))
			{
				File.Delete(_setup.File);
				_log.Trace("The file {0} was deleted.", _setup.File);
			}
			else
			{
				_log.Trace("The file {0} was not found for deletion.", _setup.File);
			}
		}

	}
}
