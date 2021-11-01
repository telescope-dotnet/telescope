using System;

namespace TeleScope.GuardClauses.Abstractions
{
	public interface INumericGuardable
	{
		T IsExact<T>(T input, T comparator, string paramName = null, string message = null) where T : IComparable;
		T IsNot<T>(T input, string paramName = null, string message = null, params T[] comparators) where T : IComparable;
		T IsNotZero<T>(T input, string paramName = null, string message = null) where T : IComparable;
		T IsLargerThanZero<T>(T input, string paramName = null, string message = null) where T : IComparable;
		T IsLarger<T>(T input, T comparator, string paramName = null, string message = null) where T : IComparable;
		T IsSmaller<T>(T input, T comparator, string paramName = null, string message = null) where T : IComparable;
	}
}
