namespace TeleScope.Connectors.Plc.Siemens
{
	/// <summary>
	/// This structure holds information to select a data source on SIEMENS S7 PLCs.
	/// </summary>
	public struct S7Selector
	{
		// -- properties

		/// <summary>
		/// Gets or sets the selected datablock.
		/// </summary>
		public int Datablock { get; }
		/// <summary>
		/// Gets or sets the bit offset within the datablock.
		/// </summary>
		public int BitOffset { get; }
		/// <summary>
		/// Gets or sets the number or index of a single bit or the number of characters to be read.
		/// </summary>
		public int Number { get; }

		// -- constructurs

		/// <summary>
		/// Creates an instance of the selector with the given parameters. The property Number will be zero.
		/// </summary>
		/// <param name="datablock">The datablock to be selected.</param>
		/// <param name="bitOffset">The bit offset to be selected.</param>
		public S7Selector(int datablock, int bitOffset)
		{
			Datablock = datablock;
			BitOffset = bitOffset;
			Number = 0;
		}

		/// <summary>
		/// Creates an instance of the selector with the given parameters.
		/// </summary>
		/// <param name="datablock">The datablock to be selected.</param>
		/// <param name="bitOffset">The bit offset to be selected.</param>
		/// <param name="count">The count of a single bit index or the number of characters.</param>
		public S7Selector(int datablock, int bitOffset, int count)
		{
			Datablock = datablock;
			BitOffset = bitOffset;
			Number = count;
		}

		// -- methods

		/// <summary>
		/// Overrides the ToString to present the properties.
		/// </summary>
		/// <returns>The combined string with property values.</returns>
		public override string ToString() => $"DB: {Datablock} Offset: {BitOffset} Count: {Number}";
	}
}
