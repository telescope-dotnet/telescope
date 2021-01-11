namespace TeleScope.Connectors.Plc.Siemens
{
	/// <summary>
	/// This class holds setup-information to connect SIEMENS S7 PLCs. 
	/// </summary>
	public class S7Setup
	{
		/// <summary>
		/// Gets or sets the IP address of the PLC.
		/// </summary>
		public string IPAddress { get; set; }

		/// <summary>
		/// Gets or sets the rack of the PLC.
		/// </summary>
		public int Rack { get; set; }

		/// <summary>
		/// Gets or sets the slot of the PLC.
		/// </summary>
		public int Slot { get; set; }

		/// <summary>
		/// Gets or sets the name of the S7 PLC.
		/// </summary>
		public string Name { get; set; }

		// -- constructor

		/// <summary>
		/// Default empty constructor that calls the constructor of the base class
		/// </summary>
		public S7Setup()
		{
			Name = "S7";
		}
	}
}
