using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Extensions;

namespace TeleScope.Persistence.Csv
{
	public class CsvStorage<T> : IReadable<T>, IWritable<T>
	{
		// -- fields

		private readonly ILogger<CsvStorage<T>> log;
		private readonly CsvStorageSetup setup;
		private readonly IParsable<T> incomingParser;
		private readonly IParsable<string[]> outgoingParser;

		// -- properties

		public bool CanCreate { get; private set; }
		public bool CanDelete { get; private set; }

		public Encoding Encoder { get; set; }

		// -- concstructor

		public CsvStorage(CsvStorageSetup setup, IParsable<T> incomingParser, IParsable<string[]> outgoingParser)
		{
			log = LoggingProvider.CreateLogger<CsvStorage<T>>();

			this.setup = setup ?? throw new ArgumentNullException(nameof(setup));
			this.incomingParser = incomingParser ?? throw new ArgumentNullException(nameof(incomingParser));
			this.outgoingParser = outgoingParser ?? throw new ArgumentNullException(nameof(outgoingParser));

			CanCreate = setup.CanCreate;
			CanDelete = setup.CanDelete;
		}

		public IEnumerable<T> Read()
		{
			List<T> result = new List<T>();

			string[] lines = File.ReadAllLines(setup.File);

			// read all rows
			for (uint i = setup.StartRow; i < lines.Length; i++)
			{
				string[] fields = lines[i].Split(setup.Separator);
				result.Add(incomingParser.Parse<string[]>(fields, (int)i, lines.Length));
			}

			log.Trace($"csv import successfull for '{setup.Filename}'");

			return result;
		}

		public void Write(IEnumerable<T> data)
		{
			try
			{
				if (!this.ValidateOrThrow(data, new FileInfo(setup.File)))
				{
					return;
				}

				// prepare data
				var csv = new StringBuilder();
				var seperator = setup.Separator.ToString();

				if (setup.HasHeader)
				{
					csv.AppendLine(setup.Header);
				}

				// append data
				int i = 0;
				foreach (T item in data)
				{
					var line = string.Join(seperator, outgoingParser.Parse<T>(item, i, data.Count()));
					csv.AppendLine(line);
					i++;
				}

				// flush to file
				File.WriteAllText(setup.File, csv.ToString(), setup.Encoder);
				log.Trace($"csv export successfull for '{setup.Filename}'");

			}
			catch (InvalidOperationException ex)
			{
				log.Critical(ex);
			}
		}
	}
}
