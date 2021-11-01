using System;
using TeleScope.GuardClauses.Abstractions;

namespace TeleScope.GuardClauses
{
	/// <summary>
	/// This class collects all implementations of the different interfaces that provide guard clause functionalities.
	/// The provider class is used by the static class <see cref="Guard"/> that provides access to the implementations.
	/// </summary>
	public sealed class GuardProvider
	{
		// -- properties

		internal IDefensiveGuardable Against { get; private set; }

		internal INumericGuardable Numeric { get; private set; }

		internal IStringGuardable String { get; private set; }

		internal ICollectionGuardable Collection { get; private set; }

		// -- constructor

		/// <summary>
		/// The default empty constructor implements the <see cref="DefaultGuard"/> class.
		/// </summary>
		public GuardProvider() : this(new DefaultGuard())
		{

		}

		/// <summary>
		/// This constructor takes an implemenation of type <see cref="DefaultGuard"/> and stores it internally.
		/// </summary>
		/// <param name="guard">The user defined implementation of guard functions.</param>
		public GuardProvider(GuardBase guard)
		{
			New(guard);
		}

		// -- setter methods per guard interface

		public void New(GuardBase guard)
		{
			var newGuard = guard ?? new DefaultGuard();

			New(newGuard as IDefensiveGuardable);
			New(newGuard as INumericGuardable);
			New(newGuard as IStringGuardable);
			New(newGuard as ICollectionGuardable);
		}

		public void New(IDefensiveGuardable guard)
		{
			Against = guard ?? throw new ArgumentNullException(nameof(guard));
		}

		public void New(INumericGuardable numericGuard)
		{
			Numeric = numericGuard ?? throw new ArgumentNullException(nameof(numericGuard));
		}

		public void New(IStringGuardable stringGuard)
		{
			String = stringGuard ?? throw new ArgumentNullException(nameof(stringGuard));
		}

		public void New(ICollectionGuardable collectionGuard)
		{
			Collection = collectionGuard ?? throw new ArgumentNullException(nameof(collectionGuard));
		}
	}
}
