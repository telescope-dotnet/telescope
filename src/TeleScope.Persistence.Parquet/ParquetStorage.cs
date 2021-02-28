using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Parquet;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Extensions;

namespace TeleScope.Persistence.Parquet
{
	public class ParquetStorage<T> : IReadable<T>, IWritable<T> where T : new()
	{
		// -- fields

		private readonly ILogger<ParquetStorage<T>> _log;
		private readonly FileInfo _info;

		// -- properties

		public bool CanCreate { get; private set; }

		public bool CanDelete { get; private set; }

		// -- constructor

		public ParquetStorage(FileInfo fileInfo, bool canCreate = true, bool canDelete = false)
		{
			_log = LoggingProvider.CreateLogger<ParquetStorage<T>>();
			_info = fileInfo;
			CanCreate = canCreate;
			CanDelete = canDelete;
		}

		// -- methods

		public IEnumerable<T> Read()
		{
			T[] data;
			using (var stream = new FileStream(_info.FullName, FileMode.Open))
			{
				data = ParquetConvert.Deserialize<T>(stream);
			}

			return data;
		}

		public void Write(IEnumerable<T> data)
		{
			try
			{
				if (!this.ValidateOrThrow(data, _info))
				{
					return;
				}

				ParquetConvert.Serialize(data, _info.FullName);
			}
			catch (InvalidOperationException ex)
			{
				_log.Critical(ex);
			}
		}
	}
}
