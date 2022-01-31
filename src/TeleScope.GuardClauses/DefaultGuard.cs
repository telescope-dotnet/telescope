using System;
using System.Runtime.CompilerServices;
using TeleScope.GuardClauses.Abstractions;

namespace TeleScope.GuardClauses
{
	/// <summary>
	/// This partial class implements the interface <see cref="IDefensiveGuardable"/>
	/// and is the first part of teh partial classes that holds the private properties.
	/// </summary>
	internal partial class DefaultGuard : GuardBase
	{
		// -- methods

		public override T Null<T>(T input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			if (input is null) 
			{
				throw new ArgumentNullException(expression, message ?? "The value must not be Null.");
			}

			return input;
		}

		public override bool False(bool input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			if (!input)
			{
				throw new ArgumentException(message ?? "The value must not be False.", expression);
			}
			return input;
		}

		public override bool True(bool input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			if (input)
			{
				throw new ArgumentException(message ?? "The value must not be True.", expression);
			}
			return input;
		}

		public override T Equality<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);		
			if (input.Equals(comparator))
			{
				throw new ArgumentException(message ?? $"The value must not be equal to {comparator}, but was {input}.", expression);
			}
			return input;
		}

		public override T Unequality<T>(T input, T comparator, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			if (!input.Equals(comparator))
			{
				throw new ArgumentException(message ?? $"The value must be equal to {comparator}, but was {input}.", expression);
			}
			return input;
		}

	}
}
