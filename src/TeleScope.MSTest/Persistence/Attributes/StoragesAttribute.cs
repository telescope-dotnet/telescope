using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using TeleScope.MSTest.Mockups;
using TeleScope.Persistence.Abstractions;
using TeleScope.Persistence.Abstractions.Enumerations;
using TeleScope.Persistence.Csv;
using TeleScope.Persistence.Json;
using TeleScope.Persistence.Parquet;
using TeleScope.Persistence.Yaml;

namespace TeleScope.MSTest.Persistence.Attributes
{
	[AttributeUsage(AttributeTargets.Method)]
	class StoragesAttribute : Attribute, ITestDataSource
	{
		public IEnumerable<object[]> GetData(MethodInfo methodInfo)
		{
			// add object array as parameters for TestMethods
			var list = new List<object[]>
			{
				BuildJson(),
				BuildYaml(),
				BuildCsv(),
				BuildParquet()
			};
			return list;
		}

		// -- helper 

		private static string GetFilename(string extension)
		{
			return Path.Combine("App_Data", $"data.{extension}");
		}

		private static object[] BuildJson()
		{
			var ext = "json";
			var file = GetFilename(ext);
			var json = new JsonStorage<Mockup>(file);
			return new object[] {
				ext,
				file,
				json as IReadable<Mockup>,
				json as IWritable<Mockup>,
			};
		}

		private static object[] BuildYaml()
		{
			var ext = "yml";
			var file = GetFilename(ext);
			var yaml = new YamlStorage<Mockup>(new YamlStorageSetup(file));
			return new object[] {
				ext,
				file,
				yaml as IReadable<Mockup>,
				yaml as IWritable<Mockup>,
			};
		}

		private static object[] BuildCsv()
		{
			var ext = "csv";
			var file = GetFilename(ext);
			var csv = new CsvStorage<Mockup>(file, (setup) => {
				setup.StartRow = 2;
				setup.Header = "This is my awesome\r\nHEADER";
			});

			csv.OnRead = (item, index, length) =>
			{
				return new Mockup
				{
					Id = int.Parse(item[0]),
					Name = item[1],
					Greetings = item[2],
					Number = double.Parse(item[3]),
					Timestamp = DateTime.Parse(item[4])
				};
			};

			csv.OnWrite = (item, index, length) =>
			{
				return new string[] {
					item.Id.ToString(),
					item.Name,
					item.Greetings,
					item.Number.ToString(),
					item.Timestamp.ToString(),
				};
			};

			return new object[] {
				ext,
				file,
				csv as IReadable<Mockup>,
				csv as IWritable<Mockup>,
			};
		}

		private static object[] BuildParquet()
		{
			var ext = "parquet";
			var file = GetFilename(ext);

			var parquet = new ParquetStorage<Mockup>(file, (setup) => {
				setup.Permissions = WritePermissions.Create | WritePermissions.Delete;
			});

			return new object[] {
				ext,
				file,
				parquet as IReadable<Mockup>,
				parquet as IWritable<Mockup>,
			};
		}

		// copy this method for other TestDataSource implementations
		public string GetDisplayName(MethodInfo methodInfo, object[] data)
		{
			if (data != null)
			{
				return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", methodInfo.Name, string.Join(",", data));
			}
			return null;
		}
	}
}
