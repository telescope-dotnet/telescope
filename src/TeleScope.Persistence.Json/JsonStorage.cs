using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;

namespace TeleScope.Persistence.Json
{
	public class JsonStorage : IStorable
	{
		private ILogger _log;
		private string _file;
		private JsonSerializerSettings _settings;

		public bool CanCreate { get; protected set; }

		public bool CanDelete { get; protected set; }

		// -- constructors

		private JsonStorage()
		{
			_log = LoggingProvider.CreateLogger<JsonStorage>();

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

		public IStorable SetFile(string file)
		{
			_file = file;
			return this;
		}

		public T Read<T>()
		{
			string json = string.Empty;
			using (StreamReader r = new StreamReader(_file))
			{
				json = r.ReadToEnd();
			}
			var obj = JsonConvert.DeserializeObject<T>(json, _settings);
			_log.Trace("Reading json successfull from {0}", _file);
			return obj;
		}

		public void Write<T>(T data)
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
					_log.Trace($"The data parameter is null, but the storage is not set as deletable.");
				}
			}

			var filename = Path.GetFileName(_file);
			var location = Path.GetFullPath(_file).Replace(filename, "");

			if (!string.IsNullOrEmpty(location) && !Directory.Exists(location) && CanCreate)
			{
				Create(location);
			}

			var json = JsonConvert.SerializeObject(data, Formatting.Indented, _settings);
			File.WriteAllText(_file, json);

		}

		private void Create(string location)
		{
			Directory.CreateDirectory(location);
			_log.Trace("Directory for json file created: {0}", _file);
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
