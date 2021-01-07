using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions.Factory;
using TeleScope.Persistence.Json;
using TeleScope.Persistence.Json.Extensions;

namespace TeleScope.MSTest.Persistence
{
	[TestClass]
	public class PersistenceTests : TestsBase
	{
		// -- fields

		private StorageFactory _factory;
		private JsonStorage _json;
		private StorageProxyBase _proxy;

		// -- overrides

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();

			var file = "output.json";
			_factory = new StorageFactory();
			_json = _factory.AddJson(file, "json-1");
			_json = new JsonStorage(file, true, true);
			_factory.Add("json-2", _json);
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- tests

		[TestMethod]
		public void JsonWriteReadDelete()
		{
			// arrange
			var file = Path.Combine("subdir", "data.json");
			var testNumber = 8.15;
			var data = new MockupData
			{
				Number = testNumber
			};

			// act & assert - write (create) 
			_json.SetFile(file).Write(data);
			Assert.IsTrue(File.Exists(file), $"The file '{file}' was not created.");

			// act & assert - read
			_proxy = _factory.CreateProxy("json-2", (s) => new JsonProxy(s));

			var result = _proxy.Read<MockupData>();
			_log.Debug("{0} returned from read method", result);
			Assert.IsNotNull(result, $"The object was not deserialized.");
			Assert.IsTrue(result.Number == testNumber, $"The object was not deserialized correctly.");

			// act & assert - write (delete)
			_factory.GetWriterAccess("json-2").Write<MockupData>(null);
			Assert.IsTrue(!File.Exists(file), $"The file '{file}' was not deleted.");
		}
	}

	class MockupData
	{
		public string Greetings { get; set; }

		public double Number { get; set; }

		public MockupData()
		{
			Greetings = "Hello World";
			Number = 47.11;
		}
	}
}
