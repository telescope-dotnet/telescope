using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions.Crude;

namespace TeleScope.Persistence.Json
{
	public class JsonCrude : CrudeBase, ICreatable, IReadable, IDeletable
	{
		private string _file;
		private ILogger _log;
		private JsonSerializerSettings _settings;

		public JsonCrude()
		{
			_log = LoggingProvider.CreateLogger<JsonCrude>();
			_settings = new JsonSerializerSettings();
			_file = "output.json";
		}

		public JsonCrude(string file) : this()
		{
			_file = file;
		}

		public JsonCrude(string file, JsonSerializerSettings settings) : this(file)
		{
			_settings = settings;
		}

		// -- methods

		public void Create(object input, params object[] parameters)
		{
			Create(input, ConvertOrThrow<string>(parameters[0]));
		}

		public void Create(object input, string file)
		{
			_file = file;
			Create(input);
		}

		public void Create(object input)
		{
			var filename = Path.GetFileName(_file);
			var location = Path.GetFullPath(_file).Replace(filename, "");

			if (!string.IsNullOrEmpty(location) &&
				!Directory.Exists(location))
			{
				Directory.CreateDirectory(location);
			}

			var json = JsonConvert.SerializeObject(input, Formatting.Indented, _settings);
			File.WriteAllText(_file, json);
			_log.Trace("Json file created: {0}", _file);
		}

		public T Read<T>(params object[] parameters)
		{	
			return Read<T>(ConvertOrThrow<string>(parameters[0]));
		}

		public T Read<T>(string file)
		{
			_file = file;
			return Read<T>();
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

		public void Delete(params object[] parameters)
		{
			Delete(ConvertOrThrow<string>(parameters[0]));
		}

		public void Delete(string file)
		{
			_file = file;
			Delete();
		}

		public void Delete()
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
