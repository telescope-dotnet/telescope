using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using TeleScope.Persistence.Abstractions.Extensions;
using System;

namespace TeleScope.Persistence.Yaml
{
	public class YamlStorage<T> : IReadable<T>, IWritable<T>
	{
		// -- fields

		private readonly ILogger<YamlStorage<T>> _log;
		private readonly YamlStorageSetup _setup;

		// -- properties

		public bool CanCreate => _setup.CanCreate;

		public bool CanDelete => _setup.CanDelete;

		// -- constructors

		public YamlStorage(YamlStorageSetup setup)
		{
			_log = LoggingProvider.CreateLogger<YamlStorage<T>>();
			_setup = setup ?? throw new ArgumentNullException(nameof(setup));
		}

		// -- methods

		public IEnumerable<T> Read()
		{
			/*
			 * see https://github.com/aaubry/YamlDotNet/wiki/Samples.DeserializeObjectGraph
			 */
			var input = File.ReadAllText(_setup.File, _setup.Encoder);

			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(PascalCaseNamingConvention.Instance)
				.Build();

			return new T[] { deserializer.Deserialize<T>(input) };
		}

		public void Write(IEnumerable<T> data)
		{
			try
			{
				if (!this.ValidateOrThrow(data, _setup.GetFileInfo()))
				{
					return;
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
				File.WriteAllText(_setup.File, yaml, _setup.Encoder);
			}
			catch (InvalidOperationException ex)
			{
				_log.Critical(ex);
			}			
		}
	}
}
