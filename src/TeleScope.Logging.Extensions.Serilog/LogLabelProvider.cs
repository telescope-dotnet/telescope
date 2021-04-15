using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serilog.Sinks.Loki.Labels;

namespace TeleScope.Logging.Extensions.Serilog
{
	class LogLabelProvider : ILogLabelProvider
	{
		private readonly List<LokiLabel> _labels;

		public LogLabelProvider((string Key, string Value)[] labels)
		{
			_labels = new List<LokiLabel>();
			SetLabels(labels);
		}

		public IList<LokiLabel> GetLabels()
		{
			return _labels;
		}

		public void SetLabels((string Key, string Value)[] labels)
		{
			_labels.AddRange(labels.Select(l => new LokiLabel(l.Key, l.Value)));
		}
	}
}
