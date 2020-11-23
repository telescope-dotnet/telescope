using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Persistence.Abstractions.Crude
{
	public abstract class CrudeBase
	{
		 
		protected T ConvertOrThrow<T>(object param)
		{
			if (param == null)
			{
				throw new ArgumentNullException("The parameter was null.");
			}
			else if (!param.GetType().IsAssignableFrom(typeof(T)))
			{
				throw new ArgumentException(
					$"Could not convert the parameter from '{param.GetType()}' to '{typeof(T)}'.");
			}
			else
			{
				return (T)Convert.ChangeType(param, typeof(T));
			}
		}
	}
}
