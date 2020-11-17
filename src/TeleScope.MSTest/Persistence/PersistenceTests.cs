using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging;
using TeleScope.Logging.Extensions.Serilog;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Json;
using TeleScope.Persistence.Json.Extensions;

namespace TeleScope.MSTest.Persistence
{
	[TestClass]
	public class PersistenceTests
	{
		private ILogger _log;

		private JsonCrude  _json;

		private CrudeProvider _crud;

		public PersistenceTests()
		{			
			LoggingProvider.Initialize(
				new LoggerFactory()
					.UseTemplate("{Timestamp: HH:mm:ss} [{Level}] - {Message}{NewLine}{Exception}")
					.UseLevel(LogLevel.Trace)
					.AddSerilogConsole());
			_log = LoggingProvider.CreateLogger<PersistenceTests>();

			var factory = new CrudeFactory();
			_json = factory.Json("output.json");
			_crud = factory.CreateProvider();
			
			var creator = _crud.Creator;
		}

		[TestMethod]
		public void JsonCrude()
		{
			_json.Create("data", "data.json", "subdir");
			
			var data = _json.Read<string>();
			_log.Debug("{0} returned", data);

			_json.Delete("data.json");
		}
	}
}
