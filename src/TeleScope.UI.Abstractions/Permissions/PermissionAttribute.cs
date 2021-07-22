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

		/// <summary>
		/// The default empty constructor instanciates the properties with default settings.
		/// </summary>
		public PermissionAttribute()
		{
			Level = 0;
			Throw = true;
			Message = "Permission denied";
		}

		/// <summary>
		/// This constructor sets the minimum permission level.
		/// It leaves the other properties at their default value.
		/// </summary>
		/// <param name="level">The minimum permission level that will pass the validation.</param>
		public PermissionAttribute(int level) : this()
		{
			Level = level;
		}

		/// <summary>
		/// This constructor sets the minimum permission level and the response level.
		/// It leaves the other properties at their default value.
		/// </summary>
		/// <param name="level">The minimum permission level that will pass the validation.</param>
		/// <param name="message">The response message that will be contained in the logging and the exception, if thrown.</param>
		public PermissionAttribute(int level, string message) : this(level)
		{
			Message = message;
		}
	}
}
