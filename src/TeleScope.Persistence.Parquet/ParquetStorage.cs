using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Parquet;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Parquet
{
	public class ParquetStorage<T> : IReadable<T>, IWritable<T> where T : new()
	{
		// -- fields

		private readonly ILogger<ParquetStorage<T>> _log;
		private readonly string _file;

		// -- properties

		public bool CanCreate { get; private set; }

		public bool CanDelete { get; private set; }

		// -- constructor

		public ParquetStorage(string file, bool canCreate = true, bool canDelete = false)
		{
			_log = LoggingProvider.CreateLogger<ParquetStorage<T>>();
			_file = file;
			CanCreate = canCreate;
			CanDelete = canDelete;
		}

		// -- methods

		public IEnumerable<T> Read()
		{
			T[] data;
			using (var stream = new FileStream(_file, FileMode.Open))
			{
				data = ParquetConvert.Deserialize<T>(stream);
			}

			return data;
		}

		public void Write(IEnumerable<T> data)
		{
			var info = new FileInfo(_file);
			if (data == null && info.Exists)
			{
				if (CanDelete) info.Delete();
				else _log.Trace($"Not allowed to delte file: {0}", _file);
				return;
			}
			else if (!CanCreate && !info.Exists)
			{
				_log.Trace($"Not allowed to create file: {0}", _file);
				return;
			}
			else if (CanCreate && !info.Directory.Exists)
			{
				info.Directory.Create();
				_log.Trace("Directory created for file: {0}", _file);
			}

			ParquetConvert.Serialize(data, _file);
		}
	}
}
