using System;
using TeleScope.Connectors.Abstractions;

namespace TeleScope.Connectors.Plc.Abstractions
{
	public interface IPlcConnectable : IConnectable
	{
		IConnectable Select(object parameter);
		IConnectable Select(object[] parameters);
		T Read<T>();
		void Write<T>(T data);
	}
}
