using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using TeleScope.GuardClauses.Abstractions;
using TeleScope.GuardClauses.Enumerations;

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
				var msg = message ?? $"The `{name}` must not be empty.";
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
				var msg = message ?? $"The `{name}` must not be a whitespace.";
				throw new ArgumentException(msg, name);
			}
			return input;
		}

		public override string IsMailAddress(string input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			var success = MailAddress.TryCreate(input, out MailAddress _);
			if (success)
			{
				return input;
			}
			else
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must be a valid mail address, but was `{input}`.";
				throw new ArgumentException(msg, name);
			}
		}

		public override MailAddress ToMailAddress(string input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			try
			{
				return new MailAddress(input);
			}
			catch (Exception ex)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must be a valid mail address, but was `{input}`.";
				throw new ArgumentException(msg, name, ex);
			}
		}

		public override string IsIpAddress(
				string input,
				InternetProtocols protocol = InternetProtocols.IPv4,
				string paramName = null,
				string message = null)
		{
			_ = Null(input, paramName, message);

			var success = IPAddress.TryParse(input, out IPAddress address);
			if (success && 
				(IsIPv4(address, protocol) || IsIPv6(address, protocol)))
			{
				return input;
			}
			else
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must be a valid {protocol} address, but was `{input}`.";
				throw new ArgumentException(msg, name);
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

		public override string IsUri(string input, UriKind kind = UriKind.RelativeOrAbsolute, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);

			var success = Uri.IsWellFormedUriString(input, kind);
			if (!success) 
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must be a valid uri, but was `{input}`.";
				throw new ArgumentException(msg, name);
			}

			return input;
		}

		public override string IsWebUri(string input, string paramName = null, string message = null) 
		{
			_ = Null(input, paramName, message);

			var success = Uri.TryCreate(input, UriKind.Absolute, out Uri uri);
			if (success && isWebUri(uri))
			{
				return input;
			}
			else 
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must be a valid web uri, but was `{input}`.";
				throw new ArgumentException(msg, name);
			}

			// -- local function

			static bool isWebUri(Uri uri) 
			{
				return uri is not null &&
					(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
			}
		}

		public override Uri ToUri(string input, string paramName = null, string message = null)
		{
			_ = Null(input, paramName, message);
			try
			{
				return new Uri(input);
			}
			catch (Exception ex)
			{
				var name = paramName ?? nameof(input);
				var msg = message ?? $"The `{name}` must be a valid uri, but was `{input}`.";
				throw new ArgumentException(msg, name, ex);
			}
		}
	}
}
