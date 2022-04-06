using TeleScope.Logging.Metrics.Abstractions;

namespace TeleScope.Logging.Metrics
{
	internal sealed class NullMetric : IMetric
	{
		// -- properties

		public long EllapsedMilliseconds { get; }

		public long AllocatedBytes { get; }

		// -- methods

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public void Stop()
		{
			// This should do nothing.
		}
	}
}
