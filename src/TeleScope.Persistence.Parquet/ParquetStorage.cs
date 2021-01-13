using System.Collections.Generic;
using System.IO;
using Parquet;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Parquet
{
	public class ParquetStorage<T> : IReadable<T>, IWritable<T> where T : new()
	{

		// -- fields

		private readonly string _file;

		// -- properties

		public bool CanCreate { get; private set; }

		public bool CanDelete { get; private set; }

		// -- constructor

		public ParquetStorage(string file)
		{
			_file = file;
			CanCreate = true;
			CanDelete = false;
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
			if (data == null)
			{
				return;
			}

			ParquetConvert.Serialize(data, _file);
		}
	}
}
