using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace TeleScope.Connectors.Abstractions.Secrets
{
	public class Secret : ISecret
	{
		public string Name { get; set; }

		public SecureString Password { get; private set; }

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
