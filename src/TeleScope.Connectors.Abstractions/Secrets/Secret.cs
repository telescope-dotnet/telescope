using System.Security;

namespace TeleScope.Connectors.Abstractions.Secrets
{
	/// <summary>
	/// Implements the `ISecret` interface to encapsulate a names and passwords within this type.
	/// </summary>
	public class Secret : ISecret
	{
		// -- properties

		/// <summary>
		/// Gets or sets the name of the secret.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the password of the secret.
		/// </summary>
		public SecureString Password { get; private set; }

		// -- constructors

		/// <summary>
		/// The default empty constructor initializes no properties.
		/// </summary>
		public Secret()
		{

		}

		/// <summary>
		/// The onstructor initializes the name and password.
		/// The password string should be disposed after calling this constructor.
		/// </summary>
		/// <param name="name">The name of the secret.</param>
		/// <param name="password">The password of the secret.</param>
		public Secret(string name, string password)
		{
			Name = name;
			SetPassword(password);
		}

		// -- methods

		/// <summary>
		/// Sets the password property in a safe way.
		/// </summary>
		/// <param name="password">The password string should be disposed after calling this method.</param>
		public void SetPassword(in string password)
		{
			unsafe
			{
				fixed (char* ptr = password)
				{
					Password = new SecureString(ptr, password.Length);
				}
			}
		}
	}
}
