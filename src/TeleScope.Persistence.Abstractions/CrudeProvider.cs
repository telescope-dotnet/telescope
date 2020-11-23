using System;
using TeleScope.Persistence.Abstractions.Crude;

namespace TeleScope.Persistence.Abstractions
{
	public class CrudeProvider : ICreatable, IReadable, IUpdatable, IDeletable
	{
		private CrudeBase _crude;

		public ICreatable Creator => (_crude is ICreatable ? _crude as ICreatable : null);
		public IReadable Reader => (_crude is IReadable ? _crude as IReadable : null);
		public IUpdatable Updater => (_crude is IUpdatable ? _crude as IUpdatable : null);
		public IDeletable Deleter => (_crude is IDeletable ? _crude as IDeletable : null);

		public CrudeProvider(CrudeBase crude)
		{
			_crude = crude;
		}

		// -- generic methods

		public T As<T>() where T : CrudeBase
		{
			return (T)Convert.ChangeType(_crude, typeof(T));
		}

		public void Create(object data)
		{
			Creator.Create(data);
		}

		public void Create(object data, params object[] parameters)
		{
			Creator.Create(data, parameters);
		}

		public T Read<T>()
		{
			return Reader.Read<T>();
		}

		public T Read<T>(params object[] parameters)
		{
			return Reader.Read<T>(parameters);
		}

		public void Update(object data)
		{
			Updater.Update(data);
		}

		public void Update(object data, params object[] parameters)
		{
			Updater.Update(data, parameters);
		}

		public void Delete()
		{
			Deleter.Delete();
		}

		public void Delete(params object[] parameters)
		{
			Deleter.Delete(parameters);
		}
	}
}
