using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Persistence.Abstractions.Crude;

namespace TeleScope.Persistence.Abstractions
{
	public class CrudeFactory
	{
		// -- properties

		public ICreatable Creator { get; private set; }
		public IReadable Reader { get; private set; }
		public IUpdatable Updater { get; private set; }
		public IDeletable Deleter { get; private set; }

		// -- methods

		public CrudeProvider CreateProvider()
		{
			return new CrudeProvider(this.Creator, this.Reader, this.Updater, this.Deleter);
		}

		public CrudeFactory Set(ICreatable creator)
		{
			Creator = creator;
			return this;
		}

		public CrudeFactory Set(IReadable reader)
		{
			Reader = reader;
			return this;
		}

		public CrudeFactory Set(IUpdatable updater)
		{
			Updater = updater;
			return this;
		}

		public CrudeFactory Set(IDeletable deleter)
		{
			Deleter = deleter;
			return this;
		}
	}
}
