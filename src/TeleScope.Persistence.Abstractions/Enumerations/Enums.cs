using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleScope.Persistence.Abstractions.Enumerations
{
	[Flags]
	public enum WritePermissions
	{
		Create,
		Delete
	}
}
