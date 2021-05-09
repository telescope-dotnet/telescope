﻿using System;
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

		private readonly ILogger<ParquetStorage<T>> log;
		private readonly FileInfo info;

		// -- properties

		public bool CanCreate { get; private set; }

		public bool CanDelete { get; private set; }

		// -- constructor

		public ParquetStorage(FileInfo fileInfo, bool canCreate = true, bool canDelete = false)
		{
			log = LoggingProvider.CreateLogger<ParquetStorage<T>>();
			info = fileInfo;
			CanCreate = canCreate;
			CanDelete = canDelete;
		}

		// -- methods

		public IEnumerable<T> Read()
		{
			T[] data;
			using (var stream = new FileStream(info.FullName, FileMode.Open))
			{
				data = ParquetConvert.Deserialize<T>(stream);
			}

			return data;
		}

		public void Write(IEnumerable<T> data)
		{
			try
			{
				if (!this.ValidateOrThrow(data, info))
				{
					return;
				}

				ParquetConvert.Serialize(data, info.FullName);
			}
			catch (InvalidOperationException ex)
			{
				log.Critical(ex);
			}
		}
	}
}
