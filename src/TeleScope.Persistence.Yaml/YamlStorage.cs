using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;
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
	public class YamlStorage<T> : IReadable<T>, IFileWritable<T>
	{
		// -- fields

		private readonly ILogger<YamlStorage<T>> log;
		private readonly YamlStorageSetup setup;

		// -- properties

		/// <summary>
		/// Gets the flags of permissions how files may be treated. 
		/// </summary>
		public WritePermissions Permissions => setup.Permissions;

		// -- constructors

		/// <summary>
		/// The constructor takes the file string as input parameter, 
		/// creates the <see cref="YamlStorageSetup"/> and allows to config the properties afterwards.
		/// </summary>
		/// <param name="file">The specific YAML file that the storage is related to.</param>
		/// <param name="config">The optional action to configure the created setup.</param>
		public YamlStorage(string file, Action<YamlStorageSetup> config = null) 
			: this(new YamlStorageSetup(file))
		{
			if (config is not null)
			{
				config(setup);
			}
		}

		/// <summary>
		/// The constructor takes the setup of type <seealso cref="YamlStorageSetup"/> as input parameter
		/// and binds the logging mechanism.
		/// </summary>
		/// <param name="yamlSetup">The setup is needed to work with a specific YAML file.</param>
		public YamlStorage(YamlStorageSetup yamlSetup) : this()
		{
			this.setup = yamlSetup ?? throw new ArgumentNullException(nameof(yamlSetup));
		}

		private YamlStorage()
		{
			log = LoggingProvider.CreateLogger<YamlStorage<T>>();
		}

		// -- methods

		/// <summary>
		/// Checks if the permission is a present flag or not. 
		/// </summary>
		/// <param name="permission">The enum that is checked.</param>
		/// <returns>True if the value is a present flag, otherwise false.</returns>
		public bool HasPermission(WritePermissions permission)
		{
			return setup.Permissions.HasFlag(permission);
		}

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
				if (!this.ValidateOrThrow(data, setup.Info()))
				{
					return;
				}

				/*
				 * see https://github.com/aaubry/YamlDotNet/wiki/Samples.SerializeObjectGraph
				 */
				var serializer = new SerializerBuilder()
					.ConfigureDefaultValuesHandling(setup.ValueHandling)
					.Build();
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

		/// <summary>
		/// Updates the reference to the internal <see cref="FileInfo"/> instance 
		/// so that the data sink can be updated. 
		/// </summary>
		/// <param name="file">The new string of the file.</param>
		/// <returns>The calling instance.</returns>
		public IFileWritable<T> Update(string file)
		{
			return Update(new FileInfo(file));
		}

		/// <summary>
		/// Updates the reference to the internal <see cref="FileInfo"/> instance 
		/// so that the data sink can be updated. 
		/// </summary>
		/// <param name="fileInfo">The new FileInfo object.</param>
		/// <returns>The calling instance.</returns>
		public IFileWritable<T> Update(FileInfo fileInfo)
		{
			setup.SetFile(fileInfo);
			return this;
		}
	}
}
