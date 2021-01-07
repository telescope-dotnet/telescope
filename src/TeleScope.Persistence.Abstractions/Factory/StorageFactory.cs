using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Abstractions.Factory
{
	public class StorageFactory
	{
		// -- fields

		private Dictionary<string, IStorable> _store;

		// -- methods

		public StorageFactory()
		{
			_store = new Dictionary<string, IStorable>();
		}

		public T CreateProxy<T>(string key, Func<IStorable, T> create) where T : StorageProxyBase
		{
			return create(GetStorageAccess(key));
		}

		public StorageFactory Add(string key, IStorable storage)
		{
			_store.Add(key, storage);
			return this;
		}

		public IStorable GetStorageAccess(string key)
		{
			return _store[key];
		}

		public IReadable GetReaderAccess(string key)
		{
			return GetStorageAccess(key);
		}

		public IWritable GetWriterAccess(string key)
		{
			return GetStorageAccess(key);
		}
	}
}
