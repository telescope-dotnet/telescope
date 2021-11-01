using System;
using System.Collections.Generic;
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
			Null(input, paramName, message);
			Message = message ?? $"The `{Name}` must have a value of `{comparator}` but  is `{input}`.";

			if (input.CompareTo(comparator) != 0)
			{
				throw new ArgumentException(Message, Name);
			}

			return input;
		}

		public override T IsNot<T>(T input, string paramName = null, string message = null, params T[] comparators)
		{
			Null(input, paramName, message);		

			foreach(var c in comparators)
			{
				compareOrThrow(c);
			}	

			return input;

			// -- local functions

			void compareOrThrow(T comparator)
			{
				if (input.CompareTo(comparator) == 0)
				{
					Message = message ?? $"The `{Name}` must NOT have a value of `{comparator}` but  is `{input}`.";
					throw new ArgumentException(Message, Name);
				}
			}
		}

		public override T IsNotZero<T>(T input, string paramName = null, string message = null)
		{
			Null(input, paramName, message);
			Message = message ?? $"The `{Name}` must not be zero.";

			if (input.CompareTo(Convert.ChangeType(0, input.GetType())) == 0)
			{
				throw new ArgumentException(Message, Name);
			}

			return input;
		}

		public override T IsSmaller<T>(T input, T comparator, string paramName = null, string message = null)
		{
			Null(input, paramName, message);
			Message = message ?? $"The `{Name}` must be larger than {comparator}, but it was {input}.";

			if (input.CompareTo(comparator) > 0)
			{
				throw new ArgumentException(Message, Name);
			}

			return input;
		}

		public override T IsLarger<T>(T input, T comparator, string paramName = null, string message = null)
		{
			Null(input, paramName, message);
			Message = message ?? $"The `{Name}` must be smaller than {comparator}, but it was {input}.";

			if (input.CompareTo(comparator) < 0)
			{
				throw new ArgumentException(Message, Name);
			}

			return input;
		}

		public override T IsLargerThanZero<T>(T input, string paramName = null, string message = null)
		{
			Null(input, paramName, message);
			Message = message ?? $"The `{Name}` must be larger than zero but is `{input}`.";
		
			if (input.CompareTo(Convert.ChangeType(0, input.GetType())) <= 0)
			{
				throw new ArgumentException(Message, Name);
			}

			return input;
		}
	}
}
