using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using TeleScope.GuardClauses.Abstractions;
using TeleScope.GuardClauses.Enumerations;

namespace TeleScope.GuardClauses
{
	/// <summary>
	/// This partial class implements the interface <see cref="IStringGuardable"/>.
	/// </summary>
	internal partial class DefaultGuard : GuardBase
	{
		public override string IsNotNullOrEmpty(string input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			if (string.IsNullOrEmpty(input))
			{
				throw new ArgumentException(message ?? $"The value must not be Null or empty.", expression);
			}
			return input;
		}

		public override string IsNotNullOrWhiteSpace(string input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			if (string.IsNullOrWhiteSpace(input))
			{
				throw new ArgumentException(message ?? $"The value must not be Null or whitespace.", expression);
			}
			return input;
		}

		public override string IsMailAddress(string input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			var success = MailAddress.TryCreate(input, out MailAddress _);
			if (success)
			{
				return input;
			}
			else
			{
				throw new ArgumentException(message ?? $"The value must be a valid mail address, but was {input}.", expression);
			}
		}

		public override MailAddress ToMailAddress(string input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			try
			{
				return new MailAddress(input);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(message ?? $"The value must be a valid mail address, but was {input}.", expression, ex);
			}
		}

		public override string IsIpAddress(
				string input,
				InternetProtocols protocol = InternetProtocols.IPv4,
				[CallerArgumentExpression("input")] string expression = null,
				string message = null)
		{
			_ = Null(input, expression, message);

			var success = IPAddress.TryParse(input, out IPAddress address);
			if (success && 
				(IsIPv4(address, protocol) || IsIPv6(address, protocol)))
			{
				return input;
			}
			else
			{
				throw new ArgumentException(message ?? $"The value must be a valid {protocol} address, but was {input}.", expression);
			}

			// -- local function

			static bool IsIPv4(IPAddress address, InternetProtocols protocol)
			{
				return address.AddressFamily == AddressFamily.InterNetwork &&
					protocol == InternetProtocols.IPv4;
			}

			static bool IsIPv6(IPAddress address, InternetProtocols protocol)
			{
				return address.AddressFamily == AddressFamily.InterNetworkV6 &&
					protocol == InternetProtocols.IPv6;
			}
		}

		public override string IsUri(
			string input, 
			UriKind kind = UriKind.RelativeOrAbsolute, 
			[CallerArgumentExpression("input")] string expression = null, 
			string message = null)
		{
			_ = Null(input, expression, message);

			var success = Uri.IsWellFormedUriString(input, kind);
			if (!success) 
			{
				throw new ArgumentException(message ?? $"The value must be a valid uri, but was {input}.", expression);
			}

			return input;
		}

		public override string IsWebUri(string input, [CallerArgumentExpression("input")] string expression = null, string message = null) 
		{
			_ = Null(input, expression, message);

			var success = Uri.TryCreate(input, UriKind.Absolute, out Uri uri);
			if (success && isWebUri(uri))
			{
				return input;
			}
			else 
			{
				throw new ArgumentException(message ?? $"The value must be a valid web uri, but was {input}.", expression);
			}

			// -- local function

			static bool isWebUri(Uri uri) 
			{
				return uri is not null &&
					(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
			}
		}

		public override Uri ToUri(string input, [CallerArgumentExpression("input")] string expression = null, string message = null)
		{
			_ = Null(input, expression, message);
			try
			{
				return new Uri(input);
			}
			catch (Exception ex)
			{
				throw new ArgumentException(message ?? $"The value must be a valid uri, but was {input}.", expression, ex);
			}
		}
	}
}
