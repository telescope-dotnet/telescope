using System;
using TeleScope.GuardClauses.Abstractions;

namespace TeleScope.GuardClauses
{
	/// <summary>
	/// This partial class implements the interface <see cref="INumericGuardable"/>.
	/// </summary>
	internal partial class DefaultGuard : GuardBase
	{
		public override T IsExact<T>(T input, T comparator, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			if (input.CompareTo(comparator) != 0)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must have a value of `{comparator}` but  is `{input}`.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

		public override T IsNot<T>(T input, string paramName = null, string message = null, params T[] comparators)
		{
			_ = Null(input, paramName, message);
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
					var name = paramName ?? nameof(input);
					var msg = message ?? $"The `{name}` must NOT have a value of `{comparator}` but  is `{input}`.";
					throw new ArgumentException(msg, name);
				}
			}
		}

		public override T IsNotZero<T>(T input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			if (input.CompareTo(Convert.ChangeType(0, input.GetType())) == 0)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must not be zero.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

		public override T IsSmaller<T>(T input, T comparator, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			if (input.CompareTo(comparator) > 0)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must be larger than {comparator}, but it was {input}.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

		public override T IsLarger<T>(T input, T comparator, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);	
			if (input.CompareTo(comparator) < 0)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must be smaller than {comparator}, but it was {input}.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

		public override T IsLargerThanZero<T>(T input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			if (input.CompareTo(Convert.ChangeType(0, input.GetType())) <= 0)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must be larger than zero but is `{input}`.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}
	}
}
