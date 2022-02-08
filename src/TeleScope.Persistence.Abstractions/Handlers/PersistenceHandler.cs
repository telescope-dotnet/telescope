namespace TeleScope.Persistence.Abstractions.Handlers
{
	/// <summary>
	/// The delegate provides a generic approach to parse data structures for incoming or outgoing dataflows.
	/// </summary>
	/// <typeparam name="Tin">The incoming type.</typeparam>
	/// <typeparam name="Tout">The outgoing type.</typeparam>
	/// <param name="item">The current item.</param>
	/// <param name="index">The index of the element.
	/// If there is only a single instance the value is 0.</param>
	/// <param name="length">The length of the collection.
	/// If there is only a single instance the value is 1.</param>
	/// <returns></returns>
	public delegate Tout PersistenceHandler<in Tin, out Tout>(Tin item, int index = 0, int length = 1);
}
