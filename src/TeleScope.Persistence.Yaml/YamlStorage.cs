using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TeleScope.Persistence.Yaml
{
	/// <summary>
	/// This class provides access to YAML files and parses the data into the application-side type T.
	/// </summary>
	/// <typeparam name="T">The type T is used application-side and can be read from the data source 
	/// or be written to the data sink.</typeparam>
	public class YamlStorage<T> : IReadable<T>, IWritable<T>
	{
		// -- fields

		private readonly ILogger<YamlStorage<T>> log;
		private readonly YamlStorageSetup setup;

		// -- properties

		/// <summary>
		/// The default behavior for file storage,
		/// if it is allowed to create files or not.
		/// </summary>
		public bool CanCreate => setup.CanCreate;

		/// <summary>
		/// The default behavior for file storage, 
		/// if it is allowed to delete files or not.
		/// </summary>
		public bool CanDelete => setup.CanDelete;

		// -- constructors

		/// <summary>
		/// The constructor takes the setup of type <seealso cref="YamlStorageSetup"/> as input parameter
		/// and binds the logging mechanism.
		/// </summary>
		/// <param name="setup">The setup is needed to work with a specific YAML file.</param>
		public YamlStorage(YamlStorageSetup setup)
		{
			this.setup = setup ?? throw new ArgumentNullException(nameof(setup));
			log = LoggingProvider.CreateLogger<YamlStorage<T>>();
		}

		// -- methods

		/// <summary>
		/// Reads a given YAML file as data source and provides a collection of type T.
		/// If there is only one data object a collection with the length one is returned.
		/// </summary>
		/// <returns>The resulting data objects of type T.</returns>
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

		/// <summary>
		/// Writes a collection of type T to a YAML file as data sink.
		/// If there is only one data object there is the need to provide a collection with one element.
		/// </summary>
		/// <param name="data">The application-side data collection of type T.</param>
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
