using TeleScope.GuardClauses.Abstractions;

namespace TeleScope.GuardClauses.Enumerations
{
	/// <summary>
	/// This enumeration is used to specify the IP version for guarding  IP strings.
	/// It is used in classes, based on the <see cref="IStringGuardable"/> interface.
	/// The .NET build-in enumerations are not used in order to prevent false parameter settings.
	/// </summary>
	public enum InternetProtocols
	{
		/// <summary>
		/// Internet Protocol version 4.
		/// </summary>
		IPv4,
		/// <summary>
		/// Internet Protocol version 6.
		/// </summary>
		IPv6
	}
}
