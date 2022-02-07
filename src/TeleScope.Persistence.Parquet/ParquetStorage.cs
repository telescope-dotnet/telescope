using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Parquet;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;
using TeleScope.Persistence.Abstractions.Extensions;

namespace TeleScope.Persistence.Parquet
{
	public class ParquetStorage<T> : IReadable<T>, IWritable<T> where T : new()
	{
		// -- fields

		private readonly ILogger<ParquetStorage<T>> log;
		private readonly ParquetStorageSetup setup;

		// -- properties

		public WritePermissions Permissions => setup.Permissions;

		// -- constructor

		public ParquetStorage(string file, Action<ParquetStorageSetup> config = null) : this(new ParquetStorageSetup(file))
		{
			if (config is not null)
			{
				config(setup);
			}
		}

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

		public IEnumerable<T> Read()
		{
			T[] data;
			using (var stream = new FileStream(setup.File, FileMode.Open))
			{
				data = ParquetConvert.Deserialize<T>(stream);
			}

			return data;
		}

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
	}
}
