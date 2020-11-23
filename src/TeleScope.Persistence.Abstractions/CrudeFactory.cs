using System;
using System.Collections.Generic;
using System.Text;
using TeleScope.Persistence.Abstractions.Crude;

namespace TeleScope.Persistence.Abstractions
{
	public class CrudeFactory
	{
		// -- fields

		private CrudeBase _crude;


		// -- methods

		public CrudeProvider CreateProvider()
		{
			return new CrudeProvider(this._crude);
		}

		public CrudeFactory Set(CrudeBase crude)
		{
			_crude = crude;
			return this;
		}
	}
}
