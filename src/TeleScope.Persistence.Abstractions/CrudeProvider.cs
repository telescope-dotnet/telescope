using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Persistence.Abstractions.Crude;

namespace TeleScope.Persistence.Abstractions
{
	public class CrudeProvider : ICreatable, IReadable, IUpdatable, IDeletable
	{
		private ICreatable _creator;
		private IReadable _reader;
		private IUpdatable _updater;
		private IDeletable _deleter;

		public CrudeProvider(ICreatable c, IReadable r, IUpdatable u, IDeletable d)
		{
			_creator = c;
			_reader = r;
			_updater = u;
			_deleter = d;
		}

		public void Create(object input)
		{
			_creator.Create(input);
		}

		public void Delete()
		{
			_deleter.Delete();
		}

		public T Read<T>()
		{
			return _reader.Read<T>();
		}

		public void Update()
		{
			_updater.Update();
		}
	}
}
