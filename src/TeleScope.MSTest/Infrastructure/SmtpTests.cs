using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Connectors.Abstractions.Secrets;
using TeleScope.Connectors.Smtp;
using TeleScope.Logging.Extensions;

namespace TeleScope.MSTest.Infrastructure
{
	[TestClass]
	public class SmtpTests : TestsBase
	{
		// -- fields

		private static bool skip;

		// -- overrides

		/// <summary>
		/// Initialize method that runs once for the entire class.
		/// </summary>
		/// <param name="context"></param>
		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			/*
             * add the "local.runsettings" as global runsettings file, if context has no data.
             */
			skip = bool.Parse(GetProperty(context, SKIP_SMTP_TESTS));
		}

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

		// -- test methods

		[TestMethod]
		public void SendEmails()
		{
			if (skip)
			{
				log.Info($"Skipping SendEmails test...");
				return;
			}

			// arrange
			var address = "";
			var copy = "";
			var password = "";
			var credentials = new Secret(address, password);

			var host = "smtp.gmail.com";
			var port = 587;

			var sender = address;
			var subject = $"Greetings from {this.GetType().FullName}";
			var body = "Hello world";

			// act
			var smtp = new SmtpConnector(host, port, credentials);
			var (total, success, failed) = smtp
				.NewMessage(sender, sender, subject, body)
				.CarbonCopy(copy)
				.BlindCarbonCopy(copy)
				.HighPriority()
				.Attach(new FileInfo("App_Data/demo.csv"))
				.Send();

			// assert
			Assert.IsTrue(failed == 0, "There should be no failure on sending the emails.");
			Assert.IsTrue(total == success, "The smtp controller should have been successfull");
		}
	}
}
