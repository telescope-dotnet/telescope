using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Extensions;

namespace TeleScope.Persistence.Json
{
	public class JsonStorage<T> : IReadable<T>, IWritable<T>
	{
		// -- fields

		private readonly ILogger<JsonStorage<T>> log;
		private readonly JsonStorageSetup setup;
		private readonly JsonSerializerSettings settings;

		// -- properties

		public bool CanCreate => setup.CanCreate;
		public bool CanDelete => setup.CanDelete;



		// -- constructors

		public JsonStorage(JsonStorageSetup setup)
		{
			this.setup = setup ?? throw new ArgumentNullException(nameof(setup));
			log = LoggingProvider.CreateLogger<JsonStorage<T>>();
			settings = new JsonSerializerSettings();
		}

		public JsonStorage(JsonStorageSetup setup, JsonSerializerSettings settings) : this(setup)
		{
			this.settings = settings;
		}

		// -- methods

		public IEnumerable<T> Read()
		{
			T result;
			using (StreamReader r = new StreamReader(setup.File))
			{
				string input = r.ReadToEnd();
				result = JsonConvert.DeserializeObject<T>(input);
			}

			log.Trace("Reading json successfull from {0}", setup.File);
			return new T[] { result };
		}

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
