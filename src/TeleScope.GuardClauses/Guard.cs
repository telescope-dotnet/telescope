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

		/// <summary>
		/// Gets methods of guard clauses that are related to defensiv or basic statements.
		/// </summary>
		public static IDefensiveGuardable Against => provider.Against;

		/// <summary>
		/// Gets methods of guard clauses that are related to numerics.
		/// </summary>
		public static INumericGuardable Numeric => provider.Numeric;

		/// <summary>
		/// Gets methods of guard clauses that are related to strings.
		/// </summary>
		public static IStringGuardable String => provider.String;

		/// <summary>
		/// Gets methods of guard clauses that are related to collections.
		/// </summary>
		public static ICollectionGuardable Collection => provider.Collection;

		// -- methods

		/// <summary>
		/// Calling this method will access the internal <see cref="GuardProvider"/> that stores the implemented guard clauses. 
		/// </summary>
		/// <returns>The internal static instance of type <see cref="GuardProvider"/>.</returns>
		public static GuardProvider Provide()
		{
			return provider;
		}
	}
}