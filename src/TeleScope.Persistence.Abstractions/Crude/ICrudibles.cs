using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Persistence.Abstractions.Crude
{

	public interface ICreatable
	{
		void Create(object input);
	}

	public interface IReadable
	{
		T Read<T>();
	}

	public interface IUpdatable
	{
		void Update();
	}

	public interface IDeletable
	{
		void Delete();
	}
}
