using TeleScope.GuardClauses.Abstractions;

namespace TeleScope.GuardClauses
{
	/// <summary>
	/// The static class is the front door to all guard clauses that are provided via separate interfaces.
	/// </summary>
	public static class Guard
	{
		// -- fields

		private static readonly GuardProvider provider = new GuardProvider();

		// -- properties

		public static IDefensiveGuardable Against => provider.Against;

		public static INumericGuardable Numeric => provider.Numeric;

		public static IStringGuardable String => provider.String;

		public static ICollectionGuardable Collection => provider.Collection;

		// -- methods

		public static GuardProvider Provide()
		{
			return provider;
		}
	}
}