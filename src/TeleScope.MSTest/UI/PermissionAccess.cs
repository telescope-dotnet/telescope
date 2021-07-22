using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.UI.Abstractions.Permissions;

namespace TeleScope.MSTest.UI
{
	static class MyPermissions
	{
		public const int Guest = 0;
		public const int User = 1;
		public const int Maintainer = 2;
		public const int Administrator = 3;
	}

	public class PermissionAccess : PermissionBase
	{
		// -- fields

		private readonly ILogger log;

		private float someFloat;

		// -- properties

		[Permission(MyPermissions.User, Message = "Not allowed to change SomeFloat", Throw = true)]
		public float SomeFloat
		{
			get
			{
				IsPermissionValid();
				return someFloat;
			}
			set
			{
				IsPermissionValid();
				Passed = true;
				someFloat = value;
			}
		}

		/// <summary>
		/// !!! Attention !!! do not use the Permission attribute this way!
		/// The Secure method can't distinguish between a calling getter or setter.
		/// </summary>
		public string SomeString
		{
			[Permission(MyPermissions.User, Throw = true)]
			get
			{
				IsPermissionValid();
				return "123";
			}
			[Permission(MyPermissions.Administrator, Message = "Not allowed to change the property 'SomeString'", Throw = true)]
			set
			{
				IsPermissionValid();
				Passed = true;
				_ = value;
			}
		}

		public bool Passed { get; set; }

		// -- constructor

		public PermissionAccess(int level)
		{
			log = LoggingProvider.CreateLogger<PermissionAccess>();
			SetPermission(level);
			log.Trace($"Permission level is {base.PermissionLevel}");

			Passed = false;
		}

		// -- methods

		[Foo(Bar = 1)]
		[Permission(Level = MyPermissions.User, Message = "You have not passed level one", Throw = false)]
		public void AccessLevelOne()
		{
			if (!IsPermissionValid())
			{
				return;
			}

			log.Info("calling SecureLevelOne() passed");
			Passed = true;
		}

		[Permission(Level = MyPermissions.Maintainer, Message = "You have not passed level two", Throw = true)]
		public void AccessLevelTwo()
		{
			IsPermissionValid();

			log.Info("calling SecureLevelTwo() passed");
			Passed = true;
		}
	}
}
