using TeleScope.Persistence.Abstractions.Crude;

namespace TeleScope.Persistence.Abstractions
{
	public class CrudeProvider : ICreatable, IReadable, IUpdatable, IDeletable
	{
		public ICreatable Creator { get; private set; }
		public IReadable Reader { get; private set; }
		public IUpdatable Updater { get; private set; }
		public IDeletable Deleter { get; private set; }

		public CrudeProvider(ICreatable c, IReadable r, IUpdatable u, IDeletable d)
		{
			Creator = c;
			Reader = r;
			Updater = u;
			Deleter = d;
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
