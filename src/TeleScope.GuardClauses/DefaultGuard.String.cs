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
			Null(input, paramName, message);
			Message = message ?? $"The `{paramName}` must not be empty.";

			if (string.IsNullOrEmpty(input))
			{
				throw new ArgumentException(Message, Name);
			}

			return input;
		}

		public override string IsNotNullOrWhiteSpace(string input, string paramName = null, string message = null)
		{
			Null(input, paramName, message);
			Message = message ?? $"The `{paramName}` must not be a whitespace.";

			if (string.IsNullOrWhiteSpace(input))
			{
				throw new ArgumentException(Message, Name);
			}

			return input;
		}
	}
}
