using System;

namespace TeleScope.Persistence.Abstractions.Enumerations
{
	[Flags]
	public enum WritePermissions
	{
		None = 0,
		Create = 1,
		Delete = 2
	}
}
