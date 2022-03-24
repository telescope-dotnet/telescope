using System;

namespace TeleScope.Connectors.Http.Abstractions
{
	/// <summary>
	/// This interface provides a generic method to invoke external functions and cache its results.
	/// </summary>
	/// <typeparam name="T">The result type T is the return value of the external call or the cached data.</typeparam>
	public interface ICacheable<T> : IDisposable
	{
		/// <summary>
		///  The implementation shall return the cached data, if the key holds it already or invokes the external function.
		/// </summary>
		/// <param name="key">The key that is used to store the result of the external function call.</param>
		/// <param name="invoke">The external function that shall be called if the cache contains no data.</param>
		/// <returns>The result of type T from cache or the external function call.</returns>
		T GetOrInvoke(string key, Func<T> invoke);
	}
}
