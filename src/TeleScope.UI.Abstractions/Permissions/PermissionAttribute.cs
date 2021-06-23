using System;

namespace TeleScope.UI.Abstractions.Permissions
{
	/// <summary>
	/// Attribute class to describe the access for methods or properties.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public class PermissionAttribute : Attribute
	{
		// -- properties

		/// <summary>
		/// Gets or sets the minimum permission level.
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// Gets or sets a boolean flag wheather the security mechanism will throw an exception or not.
		/// </summary>
		public bool Throw { get; set; }

		/// <summary>
		/// Gets or sets the message that will be used, when the security mechanism will throw an exception.
		/// </summary>
		public string Message { get; set; }

		// -- constructors

		public PermissionAttribute()
		{
			Level = 0;
			Throw = true;
		}

		public PermissionAttribute(int level) : this()
		{
			Level = level;
		}

		/// <summary>
		/// Constructor with specific minimum security level
		/// </summary>
		/// <param name="level"></param>
		public PermissionAttribute(int level, string message) : this(level)
		{
			Message = message;
		}
	}
}
