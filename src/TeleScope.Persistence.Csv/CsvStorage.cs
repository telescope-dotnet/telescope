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
	/// <summary>
	/// This class provides access to CSV files (charcater separated values) and parses the data into the application-side type T.
	/// </summary>
	/// <typeparam name="T">The type T is used application-side and can be read from the data source 
	/// or be written to the data sink.</typeparam>
	public class CsvStorage<T> : IReadable<T>, IFileWritable<T>
	{
		// -- fields

		private readonly ILogger<CsvStorage<T>> log;
		private readonly CsvStorageSetup setup;
		private readonly IParsable<T> incomingParser;
		private readonly IParsable<string[]> outgoingParser;

		// -- properties

		/// <summary>
		/// The default behavior for file storage,
		/// if it is allowed to create files or not.
		/// </summary>
		public bool CanCreate => setup.CanCreate;
		/// <summary>
		/// The default behavior for file storage, 
		/// if it is allowed to delete files or not.
		/// </summary>
		public bool CanDelete => setup.CanDelete;

		// -- concstructor

		/// <summary>
		/// The constructor takes the setup of type <seealso cref="CsvStorageSetup"/> as input parameter,
		/// parsers of type <seealso cref="IParsable{Tout}"/> for incoming and outgoing data
		/// and binds the logging mechanism. 
		/// </summary>
		/// <param name="setup">The setup is needed to work with a specific CSV file.</param>
		/// <param name="incomingParser">The incoming parser matches one CSV line with a data object of type T.</param>
		/// <param name="outgoingParser">The outgoing parser matches one data object of type T to a CSV line.</param>
		public CsvStorage(CsvStorageSetup setup, IParsable<T> incomingParser, IParsable<string[]> outgoingParser)
		{
			log = LoggingProvider.CreateLogger<CsvStorage<T>>();

			this.setup = setup ?? throw new ArgumentNullException(nameof(setup));
			this.incomingParser = incomingParser ?? throw new ArgumentNullException(nameof(incomingParser));
			this.outgoingParser = outgoingParser ?? throw new ArgumentNullException(nameof(outgoingParser));
		}

		/// <summary>
		/// Reads a given CSV file as data source and provides a collection of type T.
		/// If there is only one data object a collection with the length one is returned.
		/// </summary>
		/// <returns>The resulting data objects of type T.</returns>
		public IEnumerable<T> Read()
		{
			List<T> result = new();

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

		/// <summary>
		/// Writes a collection of type T to a CSV file as data sink.
		/// If there is only one data object there is the need to provide a collection with one element.
		/// </summary>
		/// <param name="data">The application-side data collection of type T.</param>
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

		/// <summary>
		/// Updates the reference to the FileInfo object so that the data sink can be updated. 
		/// </summary>
		/// <param name="fileInfo">The new FileInfo object.</param>
		/// <returns>The calling instance.</returns>
		public IFileWritable<T> Update(FileInfo fileInfo)
		{
			setup.SetFileInfo(fileInfo);
			return this;
		}
	}
}
