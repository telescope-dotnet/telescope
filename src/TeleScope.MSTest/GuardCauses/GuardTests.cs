using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.GuardClauses;
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
			base.Arrange();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- tests

		[TestMethod]
		public void TestDefaultGuard()
		{
			// assert the guard
			Guard.Provide().New(null);

			Assert.IsNotNull(Guard.Against);
			Assert.IsNotNull(Guard.String);
			Assert.IsNotNull(Guard.Numeric);
			Assert.IsNotNull(Guard.Collection);

			// arrange

			var myNull = default(object);
			var myTrue = true;
			var myFalse = false;
			var myZeroSpan = TimeSpan.Zero;

			var myEmptyString = "";
			var myWhiteSpace = " ";

			int myZero = 0;
			var myNumber = 42;

			var myEmptyList = Array.Empty<int>();
			var myList = new object[] { 1, true, "123", null };
			var myBrockenList = new string[] { "apple", "orange", "melon" };

			// act & assert

			TryAndCatch(() => Guard.Against.Null(myNull));
			TryAndCatch(() => Guard.Against.False(myFalse, nameof(myFalse), "Hey the guard protected us against False."));
			TryAndCatch(() => Guard.Against.True(myTrue, nameof(myTrue), "Hey the guard protected us against True."));
			TryAndCatch(() => Guard.Against.Equality(myZeroSpan, TimeSpan.Zero, nameof(myTrue), "Hey the guard protected us against two equal time spans."));
			TryAndCatch(() => Guard.Against.Unequality(myZeroSpan, new TimeSpan(1, 0, 0)));

			TryAndCatch(() => Guard.String.IsNotNullOrEmpty(myEmptyString, nameof(myEmptyString), "NO empty strings please!!!"));
			TryAndCatch(() => Guard.String.IsNotNullOrWhiteSpace(myWhiteSpace, nameof(myWhiteSpace)));

			TryAndCatch(() => Guard.Numeric.IsNotZero(myZero, nameof(myZero)));
			TryAndCatch(() => Guard.Numeric.IsSmaller(myNumber, 41));
			TryAndCatch(() => Guard.Numeric.IsLarger(myNumber, 43, nameof(myNumber), "You failed!"));
			TryAndCatch(() => Guard.Numeric.IsExact(myNumber, 24, nameof(myNumber)));
			TryAndCatch(() => Guard.Numeric.IsNot(myNumber, nameof(myNumber), null, 41, 42, 43));

			TryAndCatch(() => Guard.Collection.IsFilled(myEmptyList, nameof(myEmptyList)));
			TryAndCatch(() => Guard.Collection.IsFilled(myList, nameof(myList)));
			TryAndCatch(() => Guard.Collection.Contains(myBrockenList, "Apple", nameof(myBrockenList)));
			TryAndCatch(() => Guard.Collection.ContainsNot(myBrockenList, "melon", nameof(myBrockenList)));
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
	}
}
