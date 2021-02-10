using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;

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
			if (data == null)
			{
				if (CanDelete)
				{
					Delete();
					return;
				}
				else
				{
					_log.Trace($"The data parameter is null, but the storage has no delete permission.");
				}
			}

			var filename = Path.GetFileName(_file);
			var location = Path.GetFullPath(_file).Replace(filename, "");

			if (CanCreate &&
				!string.IsNullOrEmpty(location) &&
				!Directory.Exists(location))
			{
				Create(location);
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

		private void Create(string location)
		{
			Directory.CreateDirectory(location);
			_log.Trace("Directory created for file: {0}", _file);
		}

		private void Delete()
		{
			if (File.Exists(_file))
			{
				File.Delete(_file);
				_log.Trace("The file {0} was deleted.", _file);
			}
			else
			{
				_log.Trace("The file {0} was not found for deletion.", _file);
			}
		}
	}
}
