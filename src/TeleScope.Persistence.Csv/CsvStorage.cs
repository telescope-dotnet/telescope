using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;
using TeleScope.Persistence.Abstractions.Extensions;
using TeleScope.Persistence.Abstractions.Handlers;

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

		// -- properties

		/// <summary>
		/// Gets the flags of permissions how files may be treated. 
		/// </summary>
		public WritePermissions Permissions => setup.Permissions;

		/// <summary>
		/// The delegate handles the read operation of a CSV line and returned the targeted internal type.
		/// </summary>
		public PersistenceHandler<string[], T> OnRead { private get; set; }

		/// <summary>
		/// The delegate handles the write operation to a CSV to a string array that represents a line.
		/// The input data is the internal type of the storage.
		/// </summary>
		public PersistenceHandler<T, string[]> OnWrite { private get; set; }

		// -- concstructor

		/// <summary>
		/// The constructor takes the file string as input parameter, 
		/// creates the <see cref="CsvStorageSetup"/> and allows to config the properties afterwards.
		/// </summary>
		/// <param name="file">The specific CSV file that the storage is related to.</param>
		/// <param name="config">The optional action to configure the created setup.</param>
		public CsvStorage(string file, Action<CsvStorageSetup> config = null) 
			: this(new CsvStorageSetup(file))
		{
			if (config is not null)
			{
				config(setup);
			}
		}

		/// <summary>
		/// The constructor takes the setup of type <see cref="CsvStorageSetup"/> as input parameter
		/// and binds the logging mechanism. 
		/// </summary>
		/// <param name="csvSetup">The setup is needed to work with a specific CSV file.</param>
		public CsvStorage(CsvStorageSetup csvSetup) : this()
		{
			setup = csvSetup ?? throw new ArgumentNullException(nameof(csvSetup));
		}

		private CsvStorage()
		{
			log = LoggingProvider.CreateLogger<CsvStorage<T>>();
		}

		// -- methods

		/// <summary>
		/// Checks if the permission is a present flag or not. 
		/// </summary>
		/// <param name="permission">The enum that is checked.</param>
		/// <returns>True if the value is a present flag, otherwise false.</returns>
		public bool HasPermission(WritePermissions permission)
		{
			return setup.Permissions.HasFlag(permission);
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
			int start = (int)setup.StartRow;
			for (int i = start; i < lines.Length; i++)
			{
				string[] fields = lines[i].Split(setup.Separator);

				// TODO create useful exception handling and messaging
				result.Add(OnRead(fields, i, lines.Length));
			}

			log.Trace("CSV import successfull for {File}.", setup.Filename);

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
					// TODO create useful exception handling and messaging
					var line = string.Join(seperator, OnWrite(item, i, data.Count()));
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
		/// Updates the reference to the internal <see cref="FileInfo"/> instance 
		/// so that the data sink can be updated. 
		/// </summary>
		/// <param name="file">The new string of the file.</param>
		/// <returns>The calling instance.</returns>
		public IFileWritable<T> Update(string file)
		{
			return Update(new FileInfo(file));
		}

		/// <summary>
		/// Updates the reference to the internal <see cref="FileInfo"/> instance 
		/// so that the data sink can be updated. 
		/// </summary>
		/// <param name="fileInfo">The new FileInfo object.</param>
		/// <returns>The calling instance.</returns>
		public IFileWritable<T> Update(FileInfo fileInfo)
		{
			setup.SetFile(fileInfo);
			return this;
		}
	}
}
