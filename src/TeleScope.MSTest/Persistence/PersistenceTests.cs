using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging.Extensions;
using TeleScope.MSTest.Mockups;
using TeleScope.MSTest.Persistence.Attributes;
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
			_length = 10000;
		}

		[TestCleanup]
		public override void Cleanup()
		{
			base.Cleanup();

			if (!string.IsNullOrEmpty(_file) && File.Exists(_file))
			{
				_log.Trace($"Cleanup after test, deleting {_file}");
				File.Delete(_file);
			}
		}

		// -- tests

		[TestMethod]
		[Storages]
		public void TestStorages(
			string extension,
			string source,
			IReadable<Mockup> reader,
			IWritable<Mockup> writer)
		{
			_log.Trace($"Tesing {extension} storage.");
			_file = source;


			if (extension.Equals("json") ||
				extension.Equals("yml"))
			{
				_log.Trace($"Tesing as single.");

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
				if (writer.CanDelete)
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
				var data = Mockup.RandomArray(_length);

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
				Assert.IsTrue(result.Count() == _length);

				// delete
				writer.Write(null);
				if (writer.CanDelete)
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
