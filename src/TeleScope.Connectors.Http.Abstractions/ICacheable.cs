using System;
using System.Threading.Tasks;

namespace TeleScope.Connectors.Http.Abstractions
{
	public interface ICacheable<T>
	{
		T GetOrInvoke(string key, Func<T> invoke);
	}
}
