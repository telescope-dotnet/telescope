using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;

namespace TeleScope.UI.Abstractions.Permissions
{
	/// <summary>
	/// The abstract base class provides logic to validate the permission calling an application layer method or property.
	/// The base class needs a permission level that is given at runtime an can validate the <see cref="PermissionAttribute"/>
	/// that was given at compile time.
	/// </summary>
	public abstract class PermissionBase
	{
		// -- fields

		private ILogger log;
		private int level;

		// -- properties

		/// <summary>
		///  
		/// </summary>
		protected int PermissionLevel => level;

		// -- methods

		protected void SetPermission(int level)
		{		
			this.level = level;
		}

		protected bool IsPermissionValid([CallerMemberName] string memberName = "")
		{
			CreateLogger();

			MethodBase method = GetType().GetMethod(memberName);
			PropertyInfo prop = GetType().GetProperty(memberName);

			object[] attributes;
			if (method != null)
			{
				attributes = method.GetCustomAttributes(typeof(PermissionAttribute), true);
			}
			else if (prop != null)
			{
				attributes = prop.GetCustomAttributes(typeof(PermissionAttribute), true);
			}
			else
			{
				log.Trace($"No property or method found for the member '{memberName}'");
				return true;
			}

			if (attributes == null || attributes.Length != 1)
			{
				log.Trace($"No permission level applied to '{memberName}'");
				return true;
			}

			return ValidateOrThrow(level, (PermissionAttribute)attributes[0], memberName);
		}

		// -- helper

		private void CreateLogger()
		{
			if (log is null)
			{
				log = LoggingProvider.CreateLogger(this.GetType());
			}
		}

		/// <summary>
		/// Checks if the current level is at least the same level that was set up at the calling member
		/// </summary>
		/// <param name="currentLevel">the current level of the application or user.</param>
		/// <param name="permission">the security attributes form the calling member.</param>
		/// <param name="memberName">the calling member (method or property).</param>
		/// <returns></returns>
		private bool ValidateOrThrow(int currentLevel, PermissionAttribute permission, string memberName)
		{
			if (checkLevels())
			{
				log.Trace($"security check for {memberName} passed");
				return true;
			}
			else
			{
				string msg;
				if (string.IsNullOrEmpty(permission.Message))
				{
					msg = $"Permission denied for '{memberName}'. The user level '{currentLevel}' does not meet the requirements '{permission.Level}'.";
				}
				else
				{
					msg = permission.Message;
				}

				if (permission.Throw)
				{
					throw new MethodAccessException(msg);
				}
				else
				{
					log.Critical(msg);
				}
				return false;
			}

			// -- local function

			bool checkLevels()
			{
				if (permission.Level <= currentLevel)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
	}
}
