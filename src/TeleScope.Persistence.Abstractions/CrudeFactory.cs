using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Persistence.Abstractions.Crude;

namespace TeleScope.Persistence.Abstractions
{
	public class CrudeFactory
	{
		// -- properties

		private ICreatable _creator;
		private IReadable _reader;
		private IUpdatable _updater;
		private IDeletable _deleter;

		// -- methods

		public CrudeProvider CreateProvider()
		{
			return new CrudeProvider(this._creator, this._reader, this._updater, this._deleter);
		}

		public CrudeFactory Set(ICreatable creator)
		{
			_creator = creator;
			return this;
		}

		public CrudeFactory Set(IReadable reader)
		{
			_reader = reader;
			return this;
		}

		public CrudeFactory Set(IUpdatable updater)
		{
			_updater = updater;
			return this;
		}

		public CrudeFactory Set(IDeletable deleter)
		{
			_deleter = deleter;
			return this;
		}
	}
}
