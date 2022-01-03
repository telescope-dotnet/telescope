﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using TeleScope.MSTest.Mockups;
using TeleScope.Persistence.Abstractions;
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

		private static FileInfo GetFile(string extension)
		{
			return new FileInfo(
				Path.Combine("App_Data", $"data.{extension}"));
		}

		private static object[] BuildJson()
		{
			var ext = "json";
			var file = GetFile(ext);
			var json = new JsonStorage<Mockup>(new JsonStorageSetup(file));
			return new object[] {
				ext,
				file.FullName,
				json as IReadable<Mockup>,
				json as IWritable<Mockup>,
			};
		}

		private static object[] BuildYaml()
		{
			var ext = "yml";
			var file = GetFile(ext);
			var yaml = new YamlStorage<Mockup>(new YamlStorageSetup(file, true, true));
			return new object[] {
				ext,
				file.FullName,
				yaml as IReadable<Mockup>,
				yaml as IWritable<Mockup>,
			};
		}

		private static object[] BuildCsv()
		{
			var ext = "csv";
			var file = GetFile(ext);
			var setup = new CsvStorageSetup(file,
				startRow: 2,
				header: "This is my awesome\r\nHEADER"
			);

			var csv = new CsvStorage<Mockup>(
				setup, new CsvToMockupParser(), new MockupToCsvParser());
			return new object[] {
				ext,
				file.FullName,
				csv as IReadable<Mockup>,
				csv as IWritable<Mockup>,
			};
		}

		private static object[] BuildParquet()
		{
			var ext = "parquet";
			var file = GetFile(ext);

			var parquet = new ParquetStorage<Mockup>(file, true, true);
			return new object[] {
				ext,
				file.FullName,
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
