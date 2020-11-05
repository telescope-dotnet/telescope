using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Connectors.Abstractions;
using TeleScope.Connectors.Plc.Siemens;
using TeleScope.Logging.Abstractions;

namespace TeleScope.MSTest.Logging
{
	[TestClass]
	public class LogerTests : TestsBase
	{
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

		// -- test method

		[TestMethod]
		public void TestConsoleLogger()
		{
			var connector = new S7Connector();

			connector.Setup(new MockupSetup());

			Assert.Fail();
		}
	}

	class MockupSetup : SetupBase
	{

	}
}
