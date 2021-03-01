using System.Security;

namespace TeleScope.Connectors.Abstractions.Secrets
{
	/// <summary>
	/// This interface provides properties and metods to encapsulate a names and passwords.
	/// The password is stored in a [SecureString](https://docs.microsoft.com/de-de/dotnet/api/system.security.securestring) to increase security of sensitive data.
	/// </summary>
	public interface ISecret
	{

		// -- properties

		/// <summary>
		/// Gets or sets the name of the secret.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets the password of the secret.
		/// </summary>
		SecureString Password { get; }

		// -- properties

		/// <summary>
		/// Sets the password property in a safe way.
		/// </summary>
		/// <param name="password">The password string should be disposed after calling this method.</param>
		void SetPassword(in string password);
	}
}
