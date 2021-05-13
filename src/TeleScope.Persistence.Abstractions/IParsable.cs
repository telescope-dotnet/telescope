namespace TeleScope.Persistence.Abstractions
{
	/// <summary>
	/// This interface provides a generic approach to parse data structures for incoming or outgoing 
	/// dataflows within the <seealso cref="IReadable{T}"/> and <seealso cref="IWritable{T}"/> implementations.
	/// </summary>
	/// <typeparam name="Tout">The resulting type after parsing process.</typeparam>
	public interface IParsable<out Tout>
	{
		/// <summary>
		/// Parses the input data intor the output data with optional additional information if the data objects are stored in a collection.
		/// </summary>
		/// <typeparam name="Tin">The incoming type before the parsing process.</typeparam>
		/// <param name="input">The input data.</param>
		/// <param name="index">The index of the data instance. If the instance is part of a collection this value increases above zero.</param>
		/// <param name="length">The length of the data collection. If the instance is part of a collection this value is greater than one.</param>
		/// <returns>The output data.</returns>
		Tout Parse<Tin>(Tin input, int index = 0, int length = 1);
	}
}