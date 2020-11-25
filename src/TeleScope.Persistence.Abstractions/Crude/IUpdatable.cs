using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Persistence.Abstractions.Crude
{
	public interface IUpdatable
	{
		void Update(object data);
		void Update(object data, params object[] parameters);
	}
}
