using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Persistence.Abstractions.Crude
{
	public interface ICreatable
	{
		void Create(object data);
		void Create(object data, params object[] parameters);
	}
}
