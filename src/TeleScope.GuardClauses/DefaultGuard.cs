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

		public override T Null<T>(T input, string paramName = null, string message = null)
		{
			if (input is null) 
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must not be Null.";
				throw new ArgumentNullException(name, msg);
			}

			return input;
		}

		public override bool False(bool input, string paramName = null, string message = null)
		{
			if (!input)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The value of `{name}` must not be `False`.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

		public override bool True(bool input, string paramName = null, string message = null)
		{
			if (input)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The value of `{name}` must not be `True`.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

		public override T Equality<T>(T input, T comparator, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);		
			if (input.Equals(comparator))
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The '{name}' must not be equal to {comparator}.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

		public override T Unequality<T>(T input, T comparator, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			if (!input.Equals(comparator))
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The '{name}' must be equal to {comparator}.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

	}
}
