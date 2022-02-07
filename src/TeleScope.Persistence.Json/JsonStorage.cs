using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;
using TeleScope.Persistence.Abstractions.Extensions;

namespace TeleScope.Persistence.Json
{
	/// <summary>
	/// This class provides access to JSON files and parses the data into the application-side type T.
	/// </summary>
	/// <typeparam name="T">The type T is used application-side and can be read from the data source 
	/// or be written to the data sink.</typeparam>
	public class JsonStorage<T> : IReadable<T>, IWritable<T>
	{
		// -- fields

		private readonly ILogger<JsonStorage<T>> log;
		private readonly JsonStorageSetup setup;

		// -- properties

		public WritePermissions Permissions => setup.Permissions;

		// -- constructors
		
		public JsonStorage(string file, Action<JsonStorageSetup> config = null) : this(new JsonStorageSetup(file))
		{
			if (config is not null)
			{
				config(setup);
				ValidateSetup();
			}
		}

		/// <summary>
		/// The constructor takes the setup of type <seealso cref="JsonStorageSetup"/> as input parameter
		/// and binds the logging mechanism.
		/// </summary>
		/// <param name="jsonSetup">The setup is needed to work with a specific JSON file.</param>
		public JsonStorage(JsonStorageSetup jsonSetup) : this()
		{
			setup = jsonSetup ?? throw new ArgumentNullException(nameof(jsonSetup));
			ValidateSetup();
		}

		private JsonStorage()
		{
			log = LoggingProvider.CreateLogger<JsonStorage<T>>();
		}

		// -- methods

		private void ValidateSetup() 
		{
			if (setup.Settings is null)
			{
				setup.Settings = new JsonSerializerSettings();
			}
		}

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
		/// Reads a given JSON file as data source and provides a collection of type T.
		/// If there is only one data object a collection with the length one is returned.
		/// </summary>
		/// <returns>The resulting data objects of type T.</returns>
		public IEnumerable<T> Read()
		{
			T result;
			using (StreamReader r = new(setup.File))
			{
				string input = r.ReadToEnd();
				result = JsonConvert.DeserializeObject<T>(input, setup.Settings);
			}

			log.Trace("Reading json successfull from {File}", setup.File);
			return new T[] { result };
		}

		/// <summary>
		/// Writes a collection of type T to a JSON file as data sink.
		/// If there is only one data object there is the need to provide a collection with one element.
		/// If the collection has only one element the JSON file won't have an array as root element.
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

				string json;
				if (data.Count() == 1)
				{
					json = JsonConvert.SerializeObject(data.First(), setup.Format, setup.Settings);
				}
				else
				{
					json = JsonConvert.SerializeObject(data, setup.Format, setup.Settings);
				}

				File.WriteAllText(setup.File, json, setup.Encoder);
			}
			catch (InvalidOperationException ex)
			{
				log.Critical(ex);
			}
		}
	}
}
