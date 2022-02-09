using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
		public PersistenceHandler<string[], T> OnItemRead { private get; set; }

		/// <summary>
		/// The delegate handles the write operation to a CSV to a string array that represents a line.
		/// The input data is the internal type of the storage.
		/// </summary>
		public PersistenceHandler<T, string[]> OnItemWrite { private get; set; }

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
		/// <exception cref="InvalidOperationException">Throws an exception when the <see cref="OnItemRead"/> delegate is null.</exception>
		public IEnumerable<T> Read()
		{
			if (OnItemRead is null)
			{
				throw new InvalidOperationException($"The delegate {nameof(OnItemRead)} is null.");
			}

			string[] lines = File.ReadAllLines(setup.File);
			IEnumerable<T> result = ReadLines(lines, (int)setup.StartRow);
			log.Trace("CSV import successfull for {File}.", setup.Filename);

			return result;

			// -- local function

			List<T> ReadLines(string[] lines, int start)
			{
				List<T> result = new();
				for (int i = start; i < lines.Length; i++)
				{
					string[] fields = lines[i].Split(setup.Separator);
					result.Add(OnItemRead(fields, i, lines.Length));
				}
				return result;
			}
		}

		/// <summary>
		/// Writes a collection of type T to a CSV file as data sink.
		/// If there is only one data object there is the need to provide a collection with one element.
		/// </summary>
		/// <param name="data">The application-side data collection of type T.</param>
		/// <exception cref="InvalidOperationException">Throws an exception when the <see cref="OnItemWrite"/> delegate is null 
		/// or the write operation is not allowed.</exception> 
		public void Write(IEnumerable<T> data)
		{
			if (OnItemWrite is null)
			{
				throw new InvalidOperationException($"The delegate {nameof(OnItemWrite)} is null.");
			}

			if (!this.ValidateOrThrow(data, new FileInfo(setup.File)))
			{
				return;
			}

			StringBuilder csv = WriteData(data);
			File.WriteAllText(setup.File, csv.ToString(), setup.Encoder);
			log.Trace($"csv export successfull for '{setup.Filename}'");

			// -- local function

			StringBuilder WriteData(IEnumerable<T> data)
			{
				var csv = new StringBuilder();
				var seperator = setup.Separator.ToString();
				if (setup.HasHeader)
				{
					csv.AppendLine(setup.Header);
				}

				int i = 0;
				foreach (T item in data)
				{
					var line = string.Join(seperator, OnItemWrite(item, i, data.Count()));
					csv.AppendLine(line);
					i++;
				}

				return csv;
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
