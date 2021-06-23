using System.Runtime.CompilerServices;

namespace TeleScope.UI.Abstractions.Permissions
{
	/// <summary>
	/// The interface provides a function to secure application layer methods
	/// combined with the attribute <seealso cref="PermissionAttribute"/>.
	/// </summary>
	public interface IPermissible
	{
		/// <summary>
		/// Shall implement role-based runtime permission checks.
		/// </summary>
		/// <param name="memberName">The name of the calling method, will be set implicitly.</param>
		/// <returns>Returns 'true' when successfull, otherwise 'false'.</returns>
		bool IsPermitted(int level, [CallerMemberName] string memberName = "");
	}
}
