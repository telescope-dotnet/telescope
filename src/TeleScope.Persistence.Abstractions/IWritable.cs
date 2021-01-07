using System;
using System.Collections.Generic;
using System.Text;

namespace TeleScope.Persistence.Abstractions
{
	public interface IWritable
	{

		// -- properties

		bool CanCreate { get; }

		bool CanDelete { get; }

		// -- methods

		void Write<T>(T data);
	}
}
