using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TeleScope.Persistence.Yaml
{
	public class YamlStorage<T> : IReadable<T>, IWritable<T>
	{
		// -- fields

		private readonly ILogger<YamlStorage<T>> _log;
		private readonly string _file;

		// -- properties

		public bool CanCreate { get; private set; }

		public bool CanDelete { get; private set; }

		// -- constructors

		public YamlStorage(string file, bool canCreate = true, bool canDelete = false)
		{
			_log = LoggingProvider.CreateLogger<YamlStorage<T>>();
			_file = file;
			CanCreate = canCreate;
			CanDelete = canDelete;
		}

		// -- methods

		public IEnumerable<T> Read()
		{
			/*
			 * see https://github.com/aaubry/YamlDotNet/wiki/Samples.DeserializeObjectGraph
			 */
			var input = File.ReadAllText(_file);

			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(PascalCaseNamingConvention.Instance)
				.Build();

			return new T[] { deserializer.Deserialize<T>(input) };
		}

		public void Write(IEnumerable<T> data)
		{
			var info = new FileInfo(_file);
			if (data == null && info.Exists)
			{
				if (CanDelete) info.Delete();
				else _log.Trace($"Not allowed to delte file: {0}", _file);
				return;
			}
			else if (!CanCreate && !info.Exists)
			{
				_log.Trace($"Not allowed to create file: {0}", _file);
				return;
			}
			else if (CanCreate && !info.Directory.Exists)
			{
				info.Directory.Create();
				_log.Trace("Directory created for file: {0}", _file);
			}

			/*
			 * see https://github.com/aaubry/YamlDotNet/wiki/Samples.SerializeObjectGraph
			 */
			var serializer = new SerializerBuilder().Build();
			string yaml;
			if (data.Count() == 1)
			{
				yaml = serializer.Serialize(data.First());
			}
			else
			{
				yaml = serializer.Serialize(data);
			}
			File.WriteAllText(_file, yaml);
		}
	}
}
