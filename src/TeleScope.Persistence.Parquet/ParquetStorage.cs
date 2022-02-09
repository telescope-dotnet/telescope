using Microsoft.Extensions.Logging;
using Parquet;
using System;
using System.Collections.Generic;
using System.IO;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;
using TeleScope.Persistence.Abstractions.Extensions;

namespace TeleScope.Persistence.Parquet
{
	/// <summary>
	/// This class provides access to Parquet files and parses the data into the application-side type T.
	/// </summary>
	/// <typeparam name="T">The type T is used application-side and can be read from the data source 
	/// or be written to the data sink.</typeparam>
	public class ParquetStorage<T> : IReadable<T>, IFileWritable<T> where T : new()
	{
		// -- fields

		private readonly ILogger<ParquetStorage<T>> log;
		private readonly ParquetStorageSetup setup;

		// -- properties

		/// <summary>
		/// Gets the flags of permissions how files may be treated. 
		/// </summary>
		public WritePermissions Permissions => setup.Permissions;

		// -- constructor

		/// <summary>
		/// The constructor takes the file string as input parameter, 
		/// creates the <see cref="ParquetStorageSetup"/> and allows to config the properties afterwards.
		/// </summary>
		/// <param name="file">The specific Parquet file that the storage is related to.</param>
		public ParquetStorage(string file)
			: this(new ParquetStorageSetup(file))
		{
			
		}

		/// <summary>
		/// The constructor takes the setup of type <see cref="ParquetStorageSetup"/> as input parameter
		/// and binds the logging mechanism.
		/// </summary>
		/// <param name="parquetSetup">The setup is needed to work with a specific Parquet file.</param>
		public ParquetStorage(ParquetStorageSetup parquetSetup) : this()
		{
			setup = parquetSetup ?? throw new ArgumentNullException(nameof(parquetSetup));
		}

		private ParquetStorage()
		{
			log = LoggingProvider.CreateLogger<ParquetStorage<T>>();
		}

		// -- methods

		/// <summary>
		/// Checks if the permission is a present flag or not. 
		/// </summary>
		/// <param name="permission">The enum that is checked.</param>
		/// <returns>True if the value is a present flag, otherwise false.</returns>
		public bool HasPermission(WritePermissions permission)
		{
			return Permissions.HasFlag(permission);
		}

		/// <summary>
		/// Reads a given Parquet file as data source and provides a collection of type T.
		/// If there is only one data object a collection with the length one is returned.
		/// </summary>
		/// <returns>The resulting data objects of type T.</returns>
		public IEnumerable<T> Read()
		{
			T[] data;
			using (var stream = new FileStream(setup.File, FileMode.Open))
			{
				data = ParquetConvert.Deserialize<T>(stream);
			}

			return data;
		}

		/// <summary>
		/// Writes a collection of type T to a Parquet file as data sink.
		/// If there is only one data object there is the need to provide a collection with one element.
		/// If the collection has only one element the Parquet file won't have an array as root element.
		/// </summary>
		/// <param name="data">The application-side data collection of type T.</param>
		public void Write(IEnumerable<T> data)
		{
			try
			{
				if (!this.ValidateOrThrow(data, setup.Info()))
				{
					return;
				}

				ParquetConvert.Serialize(data, setup.File);
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
