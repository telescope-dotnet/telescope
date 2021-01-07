using System;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Abstractions.Factory
{
	public abstract class StorageProxyBase : IStorable
	{
		// -- fields

		protected IStorable _storage;

		// -- properties

		public bool CanCreate => _storage.CanCreate;

		public bool CanDelete => _storage.CanDelete;

		// -- constructor

		public StorageProxyBase(IStorable storage)
		{
			_storage = storage;
		}

		// -- generic methods

		public abstract T Read<T>();

		public abstract void Write<T>(T data);
	}
}
