using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.GuardClauses;
using TeleScope.GuardClauses.Enumerations;
using TeleScope.Logging.Extensions;

namespace TeleScope.MSTest.Entities
{
	[TestClass]
	public class GuardTests : TestsBase
	{
		// -- overrides

		[TestInitialize]
		public override void Arrange()
		{
			base.ArrangeLogging<GuardTests>();

			Guard.Provide().New(null);

			Assert.IsNotNull(Guard.Against);
			Assert.IsNotNull(Guard.String);
			Assert.IsNotNull(Guard.Numeric);
			Assert.IsNotNull(Guard.Collection);
		}

		[TestCleanup]
		public override void Cleanup()
		{
		}

		// -- tests

		[TestMethod]
		public void TestDefensiveGuard()
		{
			// arrange
			var myNull = default(object);
			var myTrue = true;
			var myFalse = false;
			var myZeroSpan = TimeSpan.Zero;

			// act & assert

			// basic units

			TryAndCatch(() => Guard.Against.Null(myNull, () => {
				// optional logging or other cool stuff.
				return new Exception("My awesome custom exception");
			}));

			TryAndCatch(() => Guard.Against.Null(" ", () => new Exception("my custom exception")));

			TryAndCatch(() => Guard.Against.Null(myNull));
			TryAndCatch(() => Guard.Against.False(myFalse));
			TryAndCatch(() => Guard.Against.True(myTrue, "truely false value", "Hey the guard protected us against True."));
			TryAndCatch(() => Guard.Against.Equality(myZeroSpan, TimeSpan.Zero, nameof(myTrue), "Hey the guard protected us against two equal time spans."));
			TryAndCatch(() => Guard.Against.Unequality(myZeroSpan, new TimeSpan(1, 0, 0)));
		}

		[TestMethod]
		public void TestNumericGuard()
		{
			// arrange
			int myZero = 0;
			var myNumber = 42;

			// act & assert
			TryAndCatch(() => Guard.Numeric.IsNotZero(myZero, nameof(myZero)));
			TryAndCatch(() => Guard.Numeric.IsSmaller(myNumber, 41));
			TryAndCatch(() => Guard.Numeric.IsLarger(myNumber, 43, nameof(myNumber), "You failed!"));
			TryAndCatch(() => Guard.Numeric.IsExact(myNumber, 24, nameof(myNumber)));
			TryAndCatch(() => Guard.Numeric.IsNot(myNumber, nameof(myNumber), null, 41, 42, 43));

			TryAndCatch(() => Guard.Numeric.IsNot(myNumber, 
				(comparator) => new Exception($"Custom error found at {comparator}"), 
				41, 42, 43));
		}

		[TestMethod]
		public void TestStringGuard()
		{
			// arrange
			var myEmptyString = "";
			var myWhiteSpace = " ";

			var myBrokenMail = "no-mail-address.com";
			var myValidMail = "awesome@mail.com";

			var myBrokenIPv4 = "300.0.0.1";
			var myIPv4 = "127.0.0.1";
			var myIPv6_1 = "2001:0db8:85a3:08d3:1319:8a2e:0370:7344";	// regular notation
			var myIPv6_2 = "2001:db8::1428:57ab";                       // short notation

			var myLocalhost = "http://127.0.0.1";
			var myBrokenUri = "http:/_127.0.0.1";
			var myBrokenWebUri = "htt_://127.0.0.1";

			// act & assert
			TryAndCatch(() => Guard.String.IsNotNullOrEmpty(myEmptyString, nameof(myEmptyString), "NO empty strings please!!!"));
			TryAndCatch(() => Guard.String.IsNotNullOrWhiteSpace(myWhiteSpace));

			TryAndCatch(() => Guard.String.IsMailAddress(myBrokenMail));
			TryAndSucceed(() => {
				var mail = Guard.String.ToMailAddress(myValidMail);
				Guard.String.IsMailAddress(mail.Address);
			},
			nameof(Guard.String.ToMailAddress));

			TryAndCatch(() => Guard.String.IsIpAddress(myBrokenIPv4, InternetProtocols.IPv4));
			TryAndCatch(() => Guard.String.IsIpAddress(myIPv4, InternetProtocols.IPv6));
			TryAndSucceed(() => Guard.String.IsIpAddress(myIPv4), nameof(Guard.String.IsIpAddress));
			TryAndSucceed(() => Guard.String.IsIpAddress(myIPv6_1, InternetProtocols.IPv6), nameof(Guard.String.IsIpAddress));
			TryAndSucceed(() => Guard.String.IsIpAddress(myIPv6_2, InternetProtocols.IPv6), nameof(Guard.String.IsIpAddress));
			TryAndCatch(() => Guard.String.IsIpAddress(myIPv6_1, InternetProtocols.IPv4));

			TryAndSucceed(() => Guard.String.IsUri(myLocalhost), nameof(Guard.String.IsUri));
			TryAndCatch(() => Guard.String.IsUri(myBrokenUri, UriKind.Absolute, nameof(myBrokenUri), "Hey the guard protected us against a broken Uri."));
			TryAndCatch(() => Guard.String.IsWebUri(myBrokenWebUri));

			TryAndSucceed(() => {
				var uri = Guard.String.ToUri(myLocalhost);
				Guard.String.IsUri(uri.ToString());
			}, 
			nameof(Guard.String.ToUri));
		}

		[TestMethod]
		public void TestCollectionGuard()
		{

			// arrange
			var myEmptyList = Array.Empty<int>();
			var myList = new object[] { 1, true, "123", null };
			var myBrockenList = new string[] { "apple", "orange", "melon" };

			// act & assert
			TryAndCatch(() => Guard.Collection.IsFilled(myEmptyList));
			TryAndCatch(() => Guard.Collection.IsFilled(myList));
			TryAndCatch(() => Guard.Collection.Contains(myBrockenList, "Apple"));
			TryAndCatch(() => Guard.Collection.ContainsNot(myBrockenList, "melon"));
			TryAndCatch(() => Guard.Collection.All(myBrockenList, i => i.Equals("apple")));
		}

		private void TryAndCatch(Action action)
		{
			try
			{
				action();
				Assert.Fail("The action should have caused an exception");
			}
			catch (Exception ex)
			{
				log.Info(ex, "Guard protected us.");
			}
		}

		private void TryAndSucceed(Action action, string name)
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				log.Critical(ex);
				Assert.Fail($"The action '{name}' should have worked!");
			}
		}
	}
}
