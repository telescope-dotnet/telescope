using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions.Factory;
using TeleScope.Persistence.Csv;
using TeleScope.Persistence.Json;
using TeleScope.Persistence.Json.Extensions;

namespace TeleScope.MSTest.Persistence
{
	[TestClass]
	public class PersistenceTests : TestsBase
	{
		// -- fields

		private StorageFactory _factory;

		// -- overrides

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();

			var file = "output.json";
			_factory = new StorageFactory();
			_factory.AddJson(file, "json-1");
			var json = new JsonStorage(file, true, true);
			_factory.Add("json-2", json);
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- tests#

		[TestMethod]
		public void CsvRead()
		{
			var setup = new CsvStorageSetup("data.csv");

			var csv = new CsvStorage(setup);

			var result = csv.Read<MockupData>();
		}

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
			var json = _factory.GetStorageAccess("json-2") as JsonStorage;
			var proxy = _factory.CreateProxy("json-2", (s) => new JsonProxy(s));

			// act & assert - write (create) 
			json.SetFile(file).Write(data);
			Assert.IsTrue(File.Exists(file), $"The file '{file}' was not created.");

			// act & assert - read

			var result = proxy.Read<MockupData>();
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
