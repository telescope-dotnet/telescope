using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging.Extensions;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Csv;
using TeleScope.Persistence.Json;
using TeleScope.Persistence.Parquet;

namespace TeleScope.MSTest.Persistence
{
	[TestClass]
	public class PersistenceTests : TestsBase
	{

		// -- overrides

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();
		}

		// -- tests

		[TestMethod]
		public void CsvCreateReadDelete()
		{
			// arrange
			var setup = new CsvStorageSetup("App_Data/data.csv");
			setup.Header = "This is my awesome\r\nHEADER";
			setup.StartRow = 2;
			var csv = new CsvStorage<Mockup>(setup)
			{
				IncomingParser = new CsvToMockupParser(),
				OutgoingParser = new MockupToCsvParser()
			};

			var data = new List<Mockup>
			{
				new Mockup
				{
					Greetings = "mockup 1",
					Number = 123
				},
				new Mockup
				{
					Greetings = "whohoo 2",
					Number = -456
				}
			};

			// act & assert: create
			csv.Write(data);
			Assert.IsTrue(File.Exists(setup.File), $"The file '{setup.File}' was not created.");

			// act & assert: read
			var result = csv.Read();
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Count() == 2);

			// act & assert: delete
			csv.Write(null);
			Assert.IsTrue(!File.Exists(setup.File), $"The file '{setup.File}' was not deleted.");
		}

		[TestMethod]
		public void JsonCreateReadDelete()
		{
			// arrange
			var file = Path.Combine("App_Data", "data.json");
			var testNumber = 4711;
			var data = new Mockup
			{
				Number = testNumber
			};

			var json = new JsonStorage<Mockup>(file, true, true);

			// act & assert - write (create) 
			json.Write(new Mockup[] { data });
			Assert.IsTrue(File.Exists(file), $"The file '{file}' was not created.");

			// act & assert - read

			var array = json.Read();
			var result = array.First();
			_log.Debug("{0} returned from read method", result);
			Assert.IsNotNull(result, $"The object was not deserialized.");
			Assert.IsTrue(result.Number == testNumber, $"The object was not deserialized correctly.");

			// act & assert - write (delete)
			json.Write(null);
			Assert.IsTrue(!File.Exists(file), $"The file '{file}' was not deleted.");
		}

		[TestMethod]
		public void TestParquet()
		{
			// arrange
			var length = 100000;
			var file = "App_Data/data.parquet";
			var parquet = new ParquetStorage<Mockup>(file);
			var data = GenerateMockupList(length);

			// act write (create)
			parquet.Write(data);

			// assert
			Assert.IsTrue(File.Exists(file), $"The parquet file '{file}' was not created.");

			File.Delete(file);
		}

		private List<Mockup> GenerateMockupList(int length)
		{
			var list = new List<Mockup>();
			var rand = new Random();
			for (int i = 0; i < length; i++)
			{
				list.Add(new Mockup { Greetings = $"Greeting no. {i}", Number = rand.Next(1000) });
			}

			return list;
		}
	}


	class Mockup
	{
		public string Greetings { get; set; }

		public int Number { get; set; }

		public Mockup()
		{
			Greetings = "Hello World";
			Number = -1;
		}
	}

	class CsvToMockupParser : IParsable<Mockup>
	{
		public Mockup Parse<Tin>(Tin input)
		{
			string[] fields = input as string[];
			return new Mockup { Number = int.Parse(fields[0]), Greetings = fields[1] };
		}
	}

	class MockupToCsvParser : IParsable<string[]>
	{
		string[] IParsable<string[]>.Parse<Tin>(Tin input)
		{
			var mockup = input as Mockup;

			return new string[] { mockup.Number.ToString(), $"Mockup says '{mockup.Greetings}'." };
		}
	}
}
