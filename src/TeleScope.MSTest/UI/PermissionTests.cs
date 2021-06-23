using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging.Extensions;

namespace TeleScope.MSTest.UI
{
	[TestClass]
	public class PermissionTests : TestsBase
	{
		private PermissionAccess tester;

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();

		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// test methods

		[TestMethod]
		public void AccessLevelOne()
		{
			// arrange
			tester = new PermissionAccess(1);

			// act
			try
			{
				tester.AccessLevelOne();
			}
			catch (Exception ex)
			{
				log.Error(ex);
				tester.Passed = false;
			}


			// assert
			Assert.IsTrue(tester.Passed);
		}

		[TestMethod]
		public void AccessLevelTwo()
		{
			// arrange
			tester = new PermissionAccess(1);

			// act
			try
			{
				tester.AccessLevelTwo();
			}
			catch (Exception ex)
			{
				log.Error(ex);
				tester.Passed = false;
			}

			// assert
			Assert.IsFalse(tester.Passed);
		}

		[TestMethod]
		public void AccessProperty()
		{
			// arrange
			var myFloat = 5.5F;
			tester = new PermissionAccess(0);

			// act
			try
			{
				tester.SomeFloat = myFloat;
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}

			// assert
			Assert.IsFalse(tester.Passed);


			// arrange
			tester = new PermissionAccess(2);

			// act
			try
			{
				tester.SomeFloat = myFloat;
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}

			// assert
			Assert.IsTrue(tester.Passed);
			Assert.AreEqual(myFloat, tester.SomeFloat);
		}
	}
}
