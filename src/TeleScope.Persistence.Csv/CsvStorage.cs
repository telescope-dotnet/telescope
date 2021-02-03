using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Csv
{
	public class CsvStorage<T> : IReadable<T>, IWritable<T>
	{

		// -- fields

		private readonly ILogger<CsvStorage<T>> _log;
		private readonly CsvStorageSetup _setup;
		private readonly IParsable<T> _incomingParser;
		private readonly IParsable<string[]> _outgoingParser;

		// -- properties

		public bool CanCreate { get; private set; }
		public bool CanDelete { get; private set; }

		// -- concstructor

		public CsvStorage(CsvStorageSetup setup, IParsable<T> incomingParser, IParsable<string[]> outgoingParser)
		{
			_log = LoggingProvider.CreateLogger<CsvStorage<T>>();

			_setup = setup ?? throw new ArgumentNullException(nameof(setup));
			_incomingParser = incomingParser ?? throw new ArgumentNullException(nameof(incomingParser));
			_outgoingParser = outgoingParser ?? throw new ArgumentNullException(nameof(outgoingParser));

			CanCreate = _setup.CanCreate;
			CanDelete = _setup.CanDelete;
		}

		public IEnumerable<T> Read()
		{
			List<T> result = new List<T>();

			string[] lines = File.ReadAllLines(_setup.File);

			// read all rows
			for (uint i = _setup.StartRow; i < lines.Length; i++)
			{
				string[] fields = lines[i].Split(_setup.Separator);
				result.Add(_incomingParser.Parse<string[]>(fields, lines.Length));
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
			else if (CanCreate &&
				     !string.IsNullOrEmpty(_setup.Location) &&
					 !Directory.Exists(_setup.Location))
			{
				Create(_setup.Location);
			}
			else if (!CanCreate &&
					 !File.Exists(_setup.File))
			{
				_log.Trace($"Not allowed to create csv file");
				return;
			}

			var csv = new StringBuilder();
			var seperator = _setup.Separator.ToString();

			if (_setup.HasHeader)
			{
				csv.AppendLine(_setup.Header);
			}

			// append data
			foreach (T item in data)
			{
				var line = string.Join(seperator, _outgoingParser.Parse<T>(item, data.Count()));
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
