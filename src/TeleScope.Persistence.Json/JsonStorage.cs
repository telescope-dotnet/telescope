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
		private readonly JsonSerializerSettings settings;

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
		/// The constructor takes the setup of type <seealso cref="JsonStorageSetup"/> as input parameter
		/// and binds the logging mechanism.
		/// </summary>
		/// <param name="setup">The setup is needed to work with a specific JSON file.</param>
		public JsonStorage(JsonStorageSetup setup)
		{
			this.setup = setup ?? throw new ArgumentNullException(nameof(setup));
			log = LoggingProvider.CreateLogger<JsonStorage<T>>();
			settings = new JsonSerializerSettings();
			settings.Converters.Add(new StringEnumConverter());
		}

		/// <summary>
		/// The constructor takes the setup of type <seealso cref="JsonStorageSetup"/>
		/// and the <seealso cref="JsonSerializerSettings"/> as input parameters
		/// and binds the logging mechanism. 
		/// </summary>
		/// <param name="setup">The setup is needed to work with a specific JSON file.</param>
		/// <param name="settings">The settings have an impact on JSOn result.</param>
		public JsonStorage(JsonStorageSetup setup, JsonSerializerSettings settings) : this(setup)
		{
			this.settings = settings;
		}

		// -- methods

		/// <summary>
		/// Reads a given JSON file as data source and provides a collection of type T.
		/// If there is only one data object a collection with the length one is returned.
		/// </summary>
		/// <returns>The resulting data objects of type T.</returns>
		public IEnumerable<T> Read()
		{
			T result;
			using (StreamReader r = new StreamReader(setup.File))
			{
				string input = r.ReadToEnd();
				result = JsonConvert.DeserializeObject<T>(input, settings);
			}

			log.Trace("Reading json successfull from {0}", setup.File);
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
				if (!this.ValidateOrThrow(data, setup.GetFileInfo()))
				{
					return;
				}

				string json;
				if (data.Count() == 1)
				{
					json = JsonConvert.SerializeObject(data.First(), Formatting.Indented, settings);
				}
				else
				{
					json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
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
