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
using System.IO;

namespace TeleScope.MSTest.Persistence
{
	[TestClass]
	public class PersistenceTests : TestsBase
	{
		// -- fields

		private JsonCrude  _jsonCrude;

		// -- overrides

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();

			var factory = new CrudeFactory();
			_jsonCrude = factory.Json("output.json");
			var provider = factory.CreateProvider();
			var creator = provider.Creator;
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- tests

		[TestMethod]
		public void JsonCrude()
		{
			// arrange
			var file = Path.Combine("subdir", "data.json");
			var testNumber = 8.15;

			// act & assert - create
			_jsonCrude.Create(new MockupData
			{
				Number = testNumber
			}, file);

			Assert.IsTrue(File.Exists(file), $"The file '{file}' was not created.");
			
			// act & assert - read
			var result = _jsonCrude.Read<MockupData>();
			_log.Debug("{0} returned", result);
			Assert.IsNotNull(result, $"The object was not deserialized.");
			Assert.IsTrue(result.Number == testNumber , $"The object was not deserialized correctly.");

			// act & assert - delete
			_jsonCrude.Delete(file);
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
