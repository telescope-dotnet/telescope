using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeleScope.Logging.Extensions;
using TeleScope.MSTest.Mockups;
using TeleScope.MSTest.Persistence.Attributes;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;
using TeleScope.Persistence.Csv;
using TeleScope.Persistence.Json;
using TeleScope.Persistence.Json.Extensions;
using TeleScope.Persistence.Yaml;

namespace TeleScope.MSTest.Persistence
{
	[TestClass]
	public class PersistenceTests : TestsBase
	{
		// fields

		private string file;
		private int length;

		// -- overrides

		[TestInitialize]
		public override void Arrange()
		{
			base.Arrange();
			length = 10000;
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();

			if (!string.IsNullOrEmpty(file) && File.Exists(file))
			{
				log.Trace($"Cleanup after test, deleting {file}");
				File.Delete(file);
			}
		}

		// -- tests

		[TestMethod]
		public void CsvDelegatesShouldFail()
		{
			// arrange
			file = Path.Combine("App_Data", "demo.csv");
			var csv = new CsvStorage<Mockup>(file);

			// act & assert
			tryThrow(() => csv.Read());
			tryThrow(() => csv.Write(default));

			// -- local function

			void tryThrow(Action call)
			{
				try
				{
					call();
					// assert
					Assert.Fail("The read operation should have feiled with a proper exception.");
				}
				catch (InvalidOperationException ex)
				{
					log.Info(ex, ex.Message);
				}
			}
		}

		[TestMethod]
		public void WriteComplexYaml()
		{
			// arrange
			file = "complex.yml";
			var yaml = new YamlStorage<object>(file, (setup) =>
			{
				setup.Permissions = WritePermissions.Delete | WritePermissions.Create;
				setup.ValueHandling = YamlDotNet.Serialization.DefaultValuesHandling.Preserve;
			});

			Assert.IsTrue(yaml.HasPermission(WritePermissions.Create));
			Assert.IsTrue(yaml.HasPermission(WritePermissions.Delete));

			var data = new
			{
				Id = 47.11,
				Name = "complex instance",
				MyNull = default(object),
				MyList = new string[] { "a", "b", "c" },
				Child = new
				{
					Foo = false,
					LogEnum = LogLevel.Information
				}
			};

			// act
			yaml.Write(new object[] { data });

			// assert
			Assert.IsTrue(File.Exists(file), "File creation failed.");
			Assert.IsTrue(File.ReadAllBytes(file).Length > 0, "File should not be empty.");
		}

		[TestMethod]
		public void WriteAndReadGenericTypes()
		{
			// -- arrange
			var data = new MockupRepository();
			data.Mockups.AddRange(Mockup.RandomArray(3));
			file = Path.Combine(APP_FOLDER, "generic.json");

			var json = new JsonStorage<MockupRepository>(file, (setup) =>
			{
				setup.Settings.Converters.Add(new StringEnumConverter());
				setup.Settings.KnownTypes(new List<Type>()
				{
					typeof(Mockup)
				});
			});

			// -- act & assert: write

			json.Write(new MockupRepository[] { data });
			Assert.IsTrue(File.Exists(file), $"The file '{file}' should has been created.");

			// -- act & assert: read

			var import = json.Read().First();

			Assert.IsNotNull(import, "The json import should not be null.");
			import.Mockups.ForEach(m =>
			{
				Assert.IsNotNull(m, "The element should not be null");
				Assert.IsFalse(string.IsNullOrEmpty(m.Name), "The name of the element should not be null or empty");
			});
		}

		[TestMethod]
		[Storages]
		public void TestStorages(
			string extension,
			string source,
			IReadable<Mockup> reader,
			IWritable<Mockup> writer)
		{
			log.Trace($"Tesing {extension} storage.");
			file = source;


			if (extension.Equals("json") ||
				extension.Equals("yml"))
			{
				log.Trace($"Tesing as single.");

				// SINGLE object act & assert: Write (create), read, delete 
				var id = 4711;
				var single = Mockup.Random(id);

				// write (create)
				writer.Write(new Mockup[] { single });
				Assert.IsTrue(File.Exists(source), $"The source '{source}' was not created.");

				// read
				var singleResult = reader.Read().First();
				Assert.IsNotNull(singleResult, $"The object was not deserialized.");
				Assert.IsTrue(singleResult.Id == id, $"The object was not deserialized correctly.");

				// delete			
				writer.Write(null);
				if (writer.HasPermission(WritePermissions.Delete))
				{
					Assert.IsFalse(File.Exists(source), $"The source '{source}' should have been deleted.");
				}
				else
				{
					Assert.IsTrue(File.Exists(source), $"The source '{source}' should NOT have been deleted.");
				}
			}
			else
			{
				// ENUMERATION act & assert: Write (create), read, delete 
				var data = Mockup.RandomArray(length);

				// write (create)
				RunWithMetrics($"Create {extension}", () =>
				{
					writer.Write(data);
				});
				Assert.IsTrue(File.Exists(source), $"The source '{source}' was not created.");

				// read
				IEnumerable<Mockup> result = default;
				RunWithMetrics($"Read {extension}", () =>
				{
					result = reader.Read();
				});
				Assert.IsNotNull(result);
				Assert.IsTrue(result.Count() == length);

				// delete
				writer.Write(null);
				if (writer.HasPermission(WritePermissions.Delete))
				{
					Assert.IsFalse(File.Exists(source), $"The source '{source}' should have been deleted.");
				}
				else
				{
					Assert.IsTrue(File.Exists(source), $"The source '{source}' should NOT have been deleted.");
				}
			}
		}



	}
}
