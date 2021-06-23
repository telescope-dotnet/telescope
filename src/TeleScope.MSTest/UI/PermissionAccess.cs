using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.UI.Abstractions.Permissions;

namespace TeleScope.MSTest.UI
{
	public class PermissionAccess : PermissionBase
	{
		// -- fields

		private readonly ILogger log;

		private readonly int userLevel;
		private float someFloat;

		// -- properties


		[Permission(1, Message = "Not allowed to change SomeFloat", Throw = true)]
		public float SomeFloat
		{
			get
			{
				IsPermitted(userLevel);
				return someFloat;
			}
			set
			{
				IsPermitted(userLevel);
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
			[Permission(1, Throw = true)]
			get
			{
				IsPermitted(userLevel);
				return "123";
			}
			[Permission(2, Message = "Not allowed to change SomeString", Throw = true)]
			set
			{
				IsPermitted(userLevel);
				Passed = true;
				_ = value;
			}
		}

		public bool Passed { get; set; }

		// -- constructor

		public PermissionAccess(int userSecurityLevel)
		{
			log = LoggingProvider.CreateLogger<PermissionAccess>();

			userLevel = userSecurityLevel;
			Passed = false;
		}

		// -- methods

		[Permission(Level = 1, Message = "You have not passed level one", Throw = true)]
		public void AccessLevelOne()
		{
			IsPermitted(userLevel);

			log.Info("calling SecureLevelOne() passed");
			Passed = true;
		}

		[Permission(Level = 2, Message = "You have not passed level two", Throw = true)]
		public void AccessLevelTwo()
		{
			IsPermitted(userLevel);

			log.Info("calling SecureLevelTwo() passed");
			Passed = true;
		}
	}
}
