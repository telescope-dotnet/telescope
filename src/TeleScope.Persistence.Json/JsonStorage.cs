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

		private readonly ILogger<JsonStorage<T>> _log;
		private readonly JsonStorageSetup _setup;
		private readonly JsonSerializerSettings _settings;

		// -- properties

		public bool CanCreate => _setup.CanCreate;
		public bool CanDelete => _setup.CanDelete;

		// -- constructors

		public JsonStorage(JsonStorageSetup setup)
		{
			_log = LoggingProvider.CreateLogger<JsonStorage<T>>();
			_setup = setup ?? throw new ArgumentNullException(nameof(setup));
			_settings = new JsonSerializerSettings();
		}

		public JsonStorage(JsonStorageSetup setup, JsonSerializerSettings settings) : this(setup)
		{
			_settings = settings;
		}

		// -- methods

		public IEnumerable<T> Read()
		{
			T result;
			using (StreamReader r = new StreamReader(_setup.File))
			{
				string input = r.ReadToEnd();
				result = JsonConvert.DeserializeObject<T>(input);
			}
			
			_log.Trace("Reading json successfull from {0}", _setup.File);
			return new T[] { result };
		}

		public void Write(IEnumerable<T> data)
		{
			try
			{
				if (!this.ValidateOrThrow(data, _setup.GetFileInfo()))
				{
					return;
				}

				string json;
				if (data.Count() == 1)
				{
					json = JsonConvert.SerializeObject(data.First(), Formatting.Indented, _settings);
				}
				else
				{
					json = JsonConvert.SerializeObject(data, Formatting.Indented, _settings);
				}

				File.WriteAllText(_setup.File, json, _setup.Encoder);
			}
			catch (InvalidOperationException ex)
			{
				_log.Critical(ex);
			}
		}
	}
}
