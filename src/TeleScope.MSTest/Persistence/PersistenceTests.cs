using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging.Extensions;
using TeleScope.MSTest.Mockups;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Csv;
using TeleScope.Persistence.Json;
using TeleScope.Persistence.Parquet;

namespace TeleScope.MSTest.Persistence
{
	[TestClass]
	public class PersistenceTests : TestsBase
	{
		// fields

		private string _file;
		private int _length;

		// -- overrides

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();
			_file = string.Empty;
			_length = 10000;
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();

			if (!string.IsNullOrEmpty(_file) && File.Exists(_file))
			{
				File.Delete(_file);
			}
		}

		// -- tests

		[TestMethod]
		public void TestCsv()
		{
			// arrange
			var setup = new CsvStorageSetup(
				csvFileInfo: new FileInfo("App_Data/data.csv"),
				startRow: 2,
				header: "This is my awesome\r\nHEADER"
			);

			var csv = new CsvStorage<Mockup>(setup, new CsvToMockupParser(), new MockupToCsvParser());

			var data = Mockup.RandomArray(_length);

			// act & assert: create
			RunWithMetrics("create csv", () =>
			{
				csv.Write(data);
			});

			Assert.IsTrue(File.Exists(setup.File), $"The file '{setup.File}' was not created.");

			// act & assert: read
			IEnumerable<Mockup> result = default;
			RunWithMetrics("read csv", () =>
			{
				result = csv.Read();
			});

			Assert.IsNotNull(result);
			Assert.IsTrue(result.Count() == _length);

			// act & assert: delete
			csv.Write(null);
			Assert.IsFalse(File.Exists(setup.File), $"The file '{setup.File}' was not deleted.");
		}

		[TestMethod]
		public void TestJson()
		{
			// arrange
			_file = Path.Combine("App_Data", "data.json");
			var id = 4711;
			var data = Mockup.Random(id);

			var json = new JsonStorage<Mockup>(_file, true, true);

			// act & assert - write (create) 
			json.Write(new Mockup[] { data });
			Assert.IsTrue(File.Exists(_file), $"The file '{_file}' was not created.");

			// act & assert - read

			var array = json.Read();
			var result = array.First();
			_log.Debug("{0} returned from read method", result);
			Assert.IsNotNull(result, $"The object was not deserialized.");
			Assert.IsTrue(result.Id == id, $"The object was not deserialized correctly.");

			// act & assert - write (delete)
			json.Write(null);
			Assert.IsFalse(File.Exists(_file), $"The file '{_file}' was not deleted.");
		}

		[TestMethod]
		public void TestParquet()
		{
			// arrange
			_file = "App_Data/data.parquet";
			var minNumber = 0.5;
			var parquet = new ParquetStorage<Mockup>(_file);
			var data = Mockup.RandomArray(_length);
			Mockup[] result = default;

			// act & assert: write (create)
			base.RunWithMetrics("create parquet", () =>
			{
				parquet.Write(data);
			});
			Assert.IsTrue(File.Exists(_file), $"The parquet file '{_file}' was not created.");

			// act & assert: read			
			base.RunWithMetrics("read parquet", () =>
			{
				result = parquet.Read()
					.Where(m => m.Number > minNumber)
					.ToArray();
			});

			Assert.IsTrue(data.Length > result.Length, "Mockup array is not filtered");
		}
	}

	class CsvToMockupParser : IParsable<Mockup>
	{
		public Mockup Parse<Tin>(Tin input, int index = 0, int length = 1)
		{
			string[] fields = input as string[];

			return new Mockup
			{
				Id = int.Parse(fields[0]),
				Name = fields[1],
				Greetings = fields[2],
				Number = double.Parse(fields[3]),
				Timestamp = DateTime.Parse(fields[4])
			};
		}
	}

	class MockupToCsvParser : IParsable<string[]>
	{
		public string[] Parse<Tin>(Tin input, int index = 0, int length = 1)
		{
			var mockup = input as Mockup;

			return new string[] {
				mockup.Id.ToString(),
				mockup.Name,
				mockup.Greetings,
				mockup.Number.ToString(),
				mockup.Timestamp.ToString(),
			};
		}
	}
}
