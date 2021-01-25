using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace TeleScope.Connectors.Abstractions.Secrets
{
	public class Secret : ISecret
	{
		// -- properties

		public string Name { get; set; }

		public SecureString Password { get; private set; }

		// -- constructors

		public Secret()
		{

		}

		public Secret(string name, string password)
		{
			Name = name;
			SetPassword(password);
		}

		// -- methods

		public void SetPassword(in string password)
		{
			unsafe
			{
				fixed (char* ptr = password)
				{
					this.Password = new SecureString(ptr, password.Length);
				}
			}
		}
	}
}
