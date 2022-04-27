namespace TeleScope.Logging.Metrics.Abstractions
{
	/// <summary>
	/// The interface provides properties that represent the metrics that shall be measured at runtime.
	/// The Implementation shall start measuring automatically after instanciation and shall stop manually 
	/// or when the instance gets disposed.
	/// </summary>
	public interface IMetric : IDisposable
	{
		// -- properties

		/// <summary>
		/// The implementation shall get the ellapsed milliseconds between autostart and stop.
		/// </summary>
		public long EllapsedMilliseconds { get; }

		/// <summary>
		/// The implementation shall get the allocated bytes between autostart and stop.
		/// </summary>
		public long AllocatedBytes { get; }

		// -- methods

		/// <summary>
		/// The implementation shall stop all measurements and provide the data through the properties.
		/// </summary>
		void Stop();
	}
}
