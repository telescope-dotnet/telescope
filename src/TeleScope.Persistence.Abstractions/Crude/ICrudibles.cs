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

	public interface IReadable
	{
		T Read<T>();
		T Read<T>(params object[] parameters);
	}

	public interface IUpdatable
	{
		void Update(object data);
		void Update(object data, params object[] parameters);
	}

	public interface IDeletable
	{
		void Delete();
		void Delete(params object[] parameters);
	}
}
