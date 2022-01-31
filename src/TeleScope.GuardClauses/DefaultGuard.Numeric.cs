using System;
using System.Runtime.CompilerServices;
using TeleScope.GuardClauses.Abstractions;

namespace TeleScope.GuardClauses
{
	/// <summary>
	/// This partial class implements the interface <see cref="INumericGuardable"/>.
	/// </summary>
	internal partial class DefaultGuard : GuardBase
	{
		public override T IsExact<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			if (input.CompareTo(comparator) != 0)
			{
				throw new ArgumentException(message ?? $"The value must be exactly {comparator}, but was {input}.", expression);
			}
			return input;
		}

		public override T IsNot<T>(T input, [CallerArgumentExpression("input")] string expression = null, string message = null, params T[] comparators)
		{
			_ = Null(input, expression, message);
			foreach (var c in comparators)
			{
				compareOrThrow(c);
			}
			return input;

			// -- local functions

			void compareOrThrow(T comparator)
			{
				if (input.CompareTo(comparator) == 0)
				{
					throw new ArgumentException(message ?? $"The value must not be {comparator}, but was {input}.", expression);
				}
			}
		}

		public override T IsNotZero<T>(T input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			if (input.CompareTo(Convert.ChangeType(0, input.GetType())) == 0)
			{
				throw new ArgumentException(message ?? $"The value must not be Zero.", expression);
			}
			return input;
		}

		public override T IsSmaller<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			if (input.CompareTo(comparator) > 0)
			{
				throw new ArgumentException(message ?? $"The value must be larger than {comparator}, but was {input}.", expression);
			}
			return input;
		}

		public override T IsLarger<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);	
			if (input.CompareTo(comparator) < 0)
			{
				throw new ArgumentException(message ?? $"The value must be smaller than {comparator}, but it was {input}.", expression);
			}
			return input;
		}

		public override T IsLargerThanZero<T>(T input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			if (input.CompareTo(Convert.ChangeType(0, input.GetType())) <= 0)
			{
				throw new ArgumentException(message ?? $"The value must be larger than zero, but was {input}.", expression);
			}
			return input;
		}
	}
}
