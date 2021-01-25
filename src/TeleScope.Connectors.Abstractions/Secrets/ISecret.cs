using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace TeleScope.Connectors.Abstractions.Secrets
{
	public interface ISecret
	{

		// -- properties

		string Name { get; set; }

		SecureString Password { get; }

		// -- properties

		void SetPassword(in string password);
	}
}
