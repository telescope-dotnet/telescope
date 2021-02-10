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
		private readonly ILogger<JsonStorage<T>> _log;
		private readonly string _file;
		private readonly JsonSerializerSettings _settings;

		public bool CanCreate { get; protected set; }
		public bool CanDelete { get; protected set; }

		// -- constructors

		private JsonStorage()
		{
			_log = LoggingProvider.CreateLogger<JsonStorage<T>>();

			_file = "output.json";
			_settings = new JsonSerializerSettings();
			CanCreate = true;
			CanDelete = true;
		}

		public JsonStorage(string file) : this()
		{
			_file = file;
		}

		public JsonStorage(string file, JsonSerializerSettings settings) : this()
		{
			_file = file;
			_settings = settings;
		}

		public JsonStorage(string file, bool canCreate, bool canDelete) : this(file)
		{
			CanCreate = canCreate;
			CanDelete = canDelete;
		}

		public JsonStorage(string file, JsonSerializerSettings settings, bool canCreate, bool canDelete) : this(file, settings)
		{
			CanCreate = canCreate;
			CanDelete = canDelete;
		}

		// -- methods

		public IEnumerable<T> Read()
		{
			T result;
			using (StreamReader r = new StreamReader(_file))
			{
				string input = r.ReadToEnd();
				result = JsonConvert.DeserializeObject<T>(input);
			}
			
			_log.Trace("Reading json successfull from {0}", _file);
			return new T[] { result };
		}


		public void Write(IEnumerable<T> data)
		{
			try
			{
				if (!this.ValidateOrThrow(data, new FileInfo(_file)))
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

				File.WriteAllText(_file, json);
			}
			catch (InvalidOperationException ex)
			{
				_log.Critical(ex);
			}
		}
	}
}
