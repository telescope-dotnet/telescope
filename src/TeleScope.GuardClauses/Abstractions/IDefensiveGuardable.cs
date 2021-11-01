namespace TeleScope.GuardClauses.Abstractions
{
	/// <summary>
	/// This interface provides methods as contract for defensive guard clauses.
	/// That means, the implemented methods should fail, if they approve the the statement of the method name.
	/// </summary>
	public interface IDefensiveGuardable
	{
		/// <summary>
		/// The implementation must check the input against null and fail, if that is the case. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		T Null<T>(T input, string paramName = null, string message = null);

		/// <summary>
		/// The implementation must check the boolean input against `False` and fail, if that is the case. 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		bool False(bool input, string paramName = null, string message = null);

		/// <summary>
		/// The implementation must check the boolean input against `True` and fail, if that is the case. 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		bool True(bool input, string paramName = null, string message = null);

		/// <summary>
		/// The implementation must check the input against an unequal comparator
		/// and fail, if both instances are unequal. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input"></param>
		/// <param name="comparator"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		T Unequality<T>(T input, T comparator, string paramName = null, string message = null);

		/// <summary>
		/// The implementation must check the input against an equal comparator
		/// and fail, if both instances are equal. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="input"></param>
		/// <param name="comparator"></param>
		/// <param name="paramName"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		T Equality<T>(T input, T comparator, string paramName = null, string message = null);
	}
}
