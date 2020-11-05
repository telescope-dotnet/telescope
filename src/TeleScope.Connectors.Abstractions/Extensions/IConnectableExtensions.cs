using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Connectors.Abstractions.Extensions
{
	public static class IConnectableExtensions
	{
		public static T ValidateSetup<T>(this IConnectable connector, SetupBase setup)
		{
			var type = typeof(T); 
			if (setup == null)
			{
				throw new ArgumentNullException(nameof(setup));
			}
			else if (setup.GetType().IsAssignableFrom(type))
			{
				return (T) Convert.ChangeType(setup, type);
			}
			else
			{
				throw new ArgumentException($"The parameter must be of type {type}", nameof(setup));
			}
		}
	}
}
