using System;

namespace TeleScope.Persistence.Abstractions.Enumerations
{
	/// <summary>
	/// The enum contains the permissions to manipulate (file) storages in terms of writing operations.
	/// </summary>
	[Flags]
	public enum WritePermissions
	{
		/// <summary>
		/// No permission to write at all.
		/// </summary>
		None = 0,
		/// <summary>
		/// Permission to write and create new storages.
		/// </summary>
		Create = 1,
		/// <summary>
		/// Permission to write and delete storages.
		/// </summary>
		Delete = 2
	}
}
