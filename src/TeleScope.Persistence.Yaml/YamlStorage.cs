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

		private readonly ILogger<YamlStorage<T>> log;
		private readonly YamlStorageSetup setup;

		// -- properties

		public bool CanCreate => setup.CanCreate;

		public bool CanDelete => setup.CanDelete;

		// -- constructors

		public YamlStorage(YamlStorageSetup setup)
		{
			this.setup = setup ?? throw new ArgumentNullException(nameof(setup));
			log = LoggingProvider.CreateLogger<YamlStorage<T>>();
		}

		// -- methods

		public IEnumerable<T> Read()
		{
			/*
			 * see https://github.com/aaubry/YamlDotNet/wiki/Samples.DeserializeObjectGraph
			 */
			var input = File.ReadAllText(setup.File, setup.Encoder);

			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(PascalCaseNamingConvention.Instance)
				.Build();

			return new T[] { deserializer.Deserialize<T>(input) };
		}

		public void Write(IEnumerable<T> data)
		{
			try
			{
				if (!this.ValidateOrThrow(data, setup.GetFileInfo()))
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
				File.WriteAllText(setup.File, yaml, setup.Encoder);
			}
			catch (InvalidOperationException ex)
			{
				log.Critical(ex);
			}			
		}
	}
}
