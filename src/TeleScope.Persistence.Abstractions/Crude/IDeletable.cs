using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Persistence.Abstractions.Crude
{
	public interface IDeletable
	{
		void Delete();
		void Delete(params object[] parameters);
	}
}
