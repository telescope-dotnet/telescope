using System;
using TeleScope.Connectors.Abstractions;

namespace TeleScope.Connectors.Plc.Abstractions
{
	public interface IPlcConnectable : IConnectable
	{
		IPlcConnectable Select(object parameter);
		IPlcConnectable Select(object[] parameters);
		T Read<T>();
		void Write<T>(T data);
	}
}
