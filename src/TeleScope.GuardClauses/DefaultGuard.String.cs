using System;
using TeleScope.GuardClauses.Abstractions;

namespace TeleScope.GuardClauses
{
	/// <summary>
	/// This partial class implements the interface <see cref="IStringGuardable"/>.
	/// </summary>
	internal partial class DefaultGuard : GuardBase
	{
		public override string IsNotNullOrEmpty(string input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			if (string.IsNullOrEmpty(input))
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{paramName}` must not be empty.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

		public override string IsNotNullOrWhiteSpace(string input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			if (string.IsNullOrWhiteSpace(input))
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{paramName}` must not be a whitespace.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}
	}
}
