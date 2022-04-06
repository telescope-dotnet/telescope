namespace TeleScope.Logging.Metrics.Abstractions
{
	public interface IMetric : IDisposable
	{
		// -- properties

		public long EllapsedMilliseconds { get; }

		public long AllocatedBytes { get; }

		// -- methods

		void Stop();
	}
}
